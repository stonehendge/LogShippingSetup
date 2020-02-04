-- Global script variables required
DECLARE @primary_server nvarchar(255)
DECLARE @secondary_server nvarchar(255)

-- Cursor level variables required
DECLARE @database nvarchar(255)
DECLARE @backup_directory nvarchar(255)
DECLARE @backup_share nvarchar(255)
DECLARE @backup_destination_directory nvarchar(255)
DECLARE @start_time_offset int


SET @primary_server = 'primary_server_template' --'SQL20081C' -- <--put yout value here
SET @secondary_server = 'secondary_server_template'        -- <--put your value here


-- Execute the following statements at the Primary to configure Log Shipping 
-- for the database @primary_server.@database,
-- The script needs to be run at the Primary in the context of the [msdb] database. 
-- ----------------------------------------------------------------------------------- 
-- Adding the Log Shipping configuration 

DECLARE @DBList TABLE
(
  [database] nvarchar(100), 
  backup_directory nvarchar(100), 
  backup_share nvarchar(100), 
  backup_destination_directory nvarchar(100),
  start_time_offset int
)

INSERT INTO @DBList ([database], backup_directory,backup_share,backup_destination_directory,start_time_offset)
VALUES ('database_ls_template'); --,'backup_directory_template','backup_share_template','backup_destination_directory_template','start_time_offset_template');

DECLARE db_cursor CURSOR FOR
SELECT [database],backup_directory, backup_share, 
backup_destination_directory, start_time_offset
FROM @DBList
--FROM [SQL-MNG2014\SQLMNG].[ManagementDBA].[dbo].LSDBList
--where [database] not in ('_ADMIN','Abakan','Arhangelsk')
--where [database]  in ('Arhangelsk')

OPEN db_cursor

FETCH NEXT FROM db_cursor INTO
@database, @backup_directory, @backup_share, 
@backup_destination_directory, @start_time_offset

WHILE @@Fetch_Status = 0
BEGIN

        DECLARE @LS_BackupJobId    AS uniqueidentifier 
        DECLARE @LS_PrimaryId    AS uniqueidentifier 
        DECLARE @SP_Add_RetCode    As int 

        DECLARE @backup_job_name nvarchar(255)
        DECLARE @backup_schedule_name nvarchar(255)
        DECLARE @copy_job_name nvarchar(255)
        DECLARE @copy_schedule_name nvarchar(255)
        DECLARE @restore_job_name nvarchar(255)
        DECLARE @restore_schedule_name nvarchar(255)
		DECLARE @backup_retention_period nvarchar(255)

		DECLARE @freq_subday_interval as int
		SET @freq_subday_interval = 'freq_subday_interval_template'
		SET @backup_retention_period = 'backup_retention_period_template'

        SET @backup_job_name = N'LSBackup_' + @database
        SET @backup_schedule_name = N'LSBackupSchedule_' + @database
        SET @copy_job_name = N'LSCopy_' + @primary_server + @database
        SET @copy_schedule_name = N'LSCopySchedule_' + @primary_server + '_' + @database
        SET @restore_job_name = N'LSRestore_' + @primary_server + '_' + @database
        SET @restore_schedule_name = 'LSRestore_Schedule_' + @primary_server + '_' + @database
        
		DECLARE @cmd_make_full_recovery nvarchar(800)

		IF EXISTS (
			select 1
			from sys.databases
			where recovery_model_desc = 'SIMPLE'
				and state_desc = 'ONLINE' and [name]=@database
        )
		BEGIN
			SET @cmd_make_full_recovery = 'ALTER DATABASE ' + @database + ' SET RECOVERY FULL;'
			EXEC sp_executesql  @cmd_make_full_recovery
		END

        EXEC @SP_Add_RetCode = master.dbo.sp_add_log_shipping_primary_database 
                @database = @database 
                ,@backup_directory = @backup_directory 
                ,@backup_share = @backup_share
                ,@backup_job_name = @backup_job_name 
                ,@backup_retention_period = @backup_retention_period
                ,@backup_threshold = 60 
                ,@threshold_alert_enabled = 1
                ,@history_retention_period = 5760 
                ,@backup_job_id = @LS_BackupJobId OUTPUT 
                ,@primary_id = @LS_PrimaryId OUTPUT 
                ,@overwrite = 1 


        IF (@@ERROR = 0 AND @SP_Add_RetCode = 0) 
        BEGIN 

        DECLARE @LS_BackUpScheduleUID    As uniqueidentifier 
        DECLARE @LS_BackUpScheduleID    AS int 


        EXEC msdb.dbo.sp_add_schedule 
                @schedule_name = @backup_schedule_name
                ,@enabled = 1 
                ,@freq_type = 4 
                ,@freq_interval = 1 
                ,@freq_subday_type = 4 
                ,@freq_subday_interval = @freq_subday_interval
                ,@freq_recurrence_factor = 0 
                ,@active_start_date = 20191210 
                ,@active_end_date = 99991231 
                ,@active_start_time = @start_time_offset 
                ,@active_end_time = 235900 
                ,@schedule_uid = @LS_BackUpScheduleUID OUTPUT 
                ,@schedule_id = @LS_BackUpScheduleID OUTPUT 

        EXEC msdb.dbo.sp_attach_schedule 
                @job_id = @LS_BackupJobId 
                ,@schedule_id = @LS_BackUpScheduleID  

        EXEC msdb.dbo.sp_update_job 
                @job_id = @LS_BackupJobId 
                ,@enabled = 1 

            

        END 


        EXEC master.dbo.sp_add_log_shipping_alert_job 

        EXEC master.dbo.sp_add_log_shipping_primary_secondary 
                @primary_database = @database
                ,@secondary_server = @secondary_server 
                ,@secondary_database = @database
                ,@overwrite = 1 


		
		

        FETCH NEXT FROM db_cursor INTO
        @database, @backup_directory, @backup_share, 
        @backup_destination_directory, @start_time_offset
    
    
        SET @LS_BackupJobId = NULL  
        SET @LS_PrimaryId    = NULL
        SET @LS_BackUpScheduleUID = NULL 
        SET @LS_BackUpScheduleID = NULL
    

END

CLOSE db_cursor;

DEALLOCATE db_cursor;

--DECLARE @sqlbackup_init varchar(4000)
--		SET @sqlbackup_init = 'sqlbackup_init_command_template'
--		EXEC sp_executesql  @sqlbackup_init
