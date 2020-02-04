-- Global script variables required
DECLARE @primary_server nvarchar(255)
DECLARE @secondary_server nvarchar(255)

-- Cursor level variables required
DECLARE @database nvarchar(255)
DECLARE @backup_directory nvarchar(255)
DECLARE @backup_share nvarchar(255)
DECLARE @backup_destination_directory nvarchar(255)
DECLARE @start_time_offset int
DECLARE @msg_error nvarchar(255)



SET @primary_server = 'primary_server_template' -- <--put yout value here
SET @secondary_server = 'secondary_server_template'       -- <--put your value here
-- ****** Begin: Script to be run at Secondary: ******

DECLARE @DBList TABLE
(
  [database] nvarchar(100), 
  backup_directory nvarchar(100), 
  backup_share nvarchar(100), 
  backup_destination_directory nvarchar(100),
  start_time_offset int
)

INSERT INTO @DBList ([database], backup_directory,backup_share,backup_destination_directory,start_time_offset)
VALUES ('database_ls_template');

DECLARE db_cursor CURSOR FOR
SELECT [database],backup_directory, backup_share, 
backup_destination_directory, start_time_offset
FROM @DBList
--FROM [SQL-MNG2014\SQLMNG].[ManagementDBA].[dbo].LSDBList
--where [database]  in ('Arhangelsk')
--where [database] not in ('_ADMIN','Nalchik2','Arhangelsk')



OPEN db_cursor

FETCH NEXT FROM db_cursor INTO
@database, @backup_directory, @backup_share, 
@backup_destination_directory, @start_time_offset

WHILE @@Fetch_Status = 0
BEGIN

		IF (NOT EXISTS (SELECT name 
			FROM master.dbo.sysdatabases 
			WHERE ('[' + name + ']' = @database 
			OR name = @database)))
		BEGIN
			SET @msg_error = 'Error - no database on server' + @database
			RAISERROR(@msg_error,16,1);
		END

        DECLARE @LS_Secondary__CopyJobId    AS uniqueidentifier 
        DECLARE @LS_Secondary__RestoreJobId    AS uniqueidentifier 
        DECLARE @LS_Secondary__SecondaryId    AS uniqueidentifier 
        DECLARE @LS_Add_RetCode    As int 


        DECLARE @backup_job_name nvarchar(255)
        DECLARE @backup_schedule_name nvarchar(255)
        DECLARE @copy_job_name nvarchar(255)
        DECLARE @copy_schedule_name nvarchar(255)
        DECLARE @restore_job_name nvarchar(255)
        DECLARE @restore_schedule_name nvarchar(255)

		DECLARE @file_retention_period int
		DECLARE @freq_subday_interval as int

		SET @file_retention_period = 'file_retention_period_template'
		SET @freq_subday_interval = 'freq_subday_interval_template'

        SET @backup_job_name = N'LSBackup_' + @database
        SET @backup_schedule_name = N'LSBackupSchedule_' + @database
        SET @copy_job_name = N'LSCopy_' + @primary_server + '_' + @database
        SET @copy_schedule_name = N'LSCopySchedule_' + @primary_server + '_' + @database
        SET @restore_job_name = N'LSRestore_' + @primary_server + '_' + @database
        SET @restore_schedule_name = 'LSRestore_Schedule_' + @primary_server + '_' + @database


        EXEC @LS_Add_RetCode = master.dbo.sp_add_log_shipping_secondary_primary 
                @primary_server = @primary_server
                ,@primary_database = @database 
                ,@backup_source_directory = @backup_share 
                ,@backup_destination_directory = @backup_destination_directory
                ,@copy_job_name = @copy_job_name
                ,@restore_job_name = @restore_job_name 
                ,@file_retention_period = @file_retention_period
                ,@overwrite = 1 
                ,@copy_job_id = @LS_Secondary__CopyJobId OUTPUT 
                ,@restore_job_id = @LS_Secondary__RestoreJobId OUTPUT 
                ,@secondary_id = @LS_Secondary__SecondaryId OUTPUT 

        IF (@@ERROR = 0 AND @LS_Add_RetCode = 0) 
        BEGIN 

        DECLARE @LS_SecondaryCopyJobScheduleUID    As uniqueidentifier 
        DECLARE @LS_SecondaryCopyJobScheduleID    AS int 


        EXEC msdb.dbo.sp_add_schedule 
                @schedule_name =@copy_schedule_name
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
                ,@schedule_uid = @LS_SecondaryCopyJobScheduleUID OUTPUT 
                ,@schedule_id = @LS_SecondaryCopyJobScheduleID OUTPUT 

        EXEC msdb.dbo.sp_attach_schedule 
                @job_id = @LS_Secondary__CopyJobId 
                ,@schedule_id = @LS_SecondaryCopyJobScheduleID  

        DECLARE @LS_SecondaryRestoreJobScheduleUID    As uniqueidentifier 
        DECLARE @LS_SecondaryRestoreJobScheduleID    AS int 


        EXEC msdb.dbo.sp_add_schedule 
                @schedule_name =@restore_schedule_name
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
                ,@schedule_uid = @LS_SecondaryRestoreJobScheduleUID OUTPUT 
                ,@schedule_id = @LS_SecondaryRestoreJobScheduleID OUTPUT 

        EXEC msdb.dbo.sp_attach_schedule 
                @job_id = @LS_Secondary__RestoreJobId 
                ,@schedule_id = @LS_SecondaryRestoreJobScheduleID  


        END 


        DECLARE @LS_Add_RetCode2    As int 


        IF (@@ERROR = 0 AND @LS_Add_RetCode = 0) 
        BEGIN 

        EXEC @LS_Add_RetCode2 = master.dbo.sp_add_log_shipping_secondary_database 
                @secondary_database = @database
                ,@primary_server = @primary_server
                ,@primary_database = @database
                ,@restore_delay = 0 
                ,@restore_mode = 0 
                ,@disconnect_users    = 0 
                ,@restore_threshold = 30   
                ,@threshold_alert_enabled = 1 
                ,@history_retention_period    = 5760 
                ,@overwrite = 1 

        END 


        IF (@@error = 0 AND @LS_Add_RetCode = 0) 
        BEGIN 

        EXEC msdb.dbo.sp_update_job 
                @job_id = @LS_Secondary__CopyJobId 
                ,@enabled = 1 

        EXEC msdb.dbo.sp_update_job 
                @job_id = @LS_Secondary__RestoreJobId 
                ,@enabled = 1 

        END 

        FETCH NEXT FROM db_cursor INTO
        @database, @backup_directory, @backup_share, 
        @backup_destination_directory, @start_time_offset

                SET @LS_Secondary__CopyJobId = NULL 
                SET @LS_Secondary__RestoreJobId = NULL 
                SET @LS_Secondary__SecondaryId = NULL
                SET @LS_SecondaryCopyJobScheduleUID = NULL
                SET @LS_SecondaryCopyJobScheduleID = NULL
                SET @LS_SecondaryRestoreJobScheduleUID = NULL
                SET @LS_SecondaryRestoreJobScheduleID = NULL

END

CLOSE db_cursor;

DEALLOCATE db_cursor;



--DECLARE @sqlrestore_init varchar(4000)
--		SET @sqlrestore_init = 'sqlrestore_init_command_template'
--		EXEC sp_executesql  @sqlrestore_init

-- ****** End: Script to be run at Secondary: ******


