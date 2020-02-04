SET NOCOUNT ON
-- This script should be run on the primary to generate a restore command for each database.
-- The restore commands that are generated should then be run against the primary after the 
-- full backup files have been copied to the secondary server 
DECLARE 
@dbid int
,@dbname varchar(1000)
,@dbfile varchar(4000)
,@dbfileonly varchar(4000)
,@dbfilename varchar(4000)
,@sqlrestore nvarchar(4000)
,@dbfiledest varchar(4000)
,@logfiledest varchar(4000) 
,@dbfilelocal varchar(4000)
,@logfilelocal varchar(4000) 
,@backup_path varchar(4000) 


DECLARE @DBList TABLE
(
  [database] nvarchar(100)
)

DECLARE @RestoreCmd TABLE
(
  cmd_restore nvarchar(max)
)

INSERT INTO @DBList ([database])
VALUES ('database_ls_template');

-- Set data and log file destinations --
Select
@backup_path = 'backup_share_template'  -- Path to the full backup 
,@dbfilelocal  = 'dbfile_mdf_template' -- Data file location on the secondary 
,@logfilelocal = 'logfile_ldf_template' -- Log file location on the secondary 

DECLARE cRestore CURSOR
READ_ONLY
FOR 
SELECT
    S.[DATABASE_ID]
    ,S.[name]
FROM 
    SYS.Databases S
    Inner Join @DBList LDB
    On S.[name] = LDB.[database]
OPEN cRestore

FETCH NEXT FROM cRestore INTO @dbid, @dbname
WHILE (@@fetch_status <> -1)
BEGIN
    -- add data files to restore script
    DECLARE cDBFiles CURSOR
    READ_ONLY
    FOR SELECT SF.[name]
    ,COALESCE(RIGHT([filename], NullIf(CHARINDEX(REVERSE('\'), REVERSE([filename])), 0)-1),
         [filename])as fileonly
    FROM 
    SYS.Databases S

    INNER JOIN sys.sysaltfiles SF
    ON S.[database_id] = SF.[dbid]
    WHERE  SF.groupid <>0 and SF.dbid = @dbid
    open cDBFiles

    FETCH NEXT FROM cDBFiles INTO @dbfilename,@dbfileonly

    SET @sqlrestore = 'RESTORE DATABASE [' + @dbname + '] FROM DISK = ''' 
    + @backup_path + '\FULL_INIT_' + @dbname + '.bak'' WITH '

    WHILE (@@fetch_status <>-1 )
    BEGIN
        -- Add file to restore script
        SET @sqlrestore = @sqlrestore + 'MOVE ''' + @dbfilename + ''' to ''' 
        + @dbfilelocal + '\' + @dbfileonly + ''''

        FETCH NEXT FROM cDBFiles INTO @dbfilename,@dbfileonly

        IF (@@fetch_status <> -1) 
            SET    @sqlrestore = @sqlrestore + ', '
        Else
            SET @sqlrestore = @sqlrestore + ', '
    END
    close cDBFiles
    deallocate cDBFiles

    -- add log files to restore script
    DECLARE cDBFiles CURSOR
    READ_ONLY
    FOR SELECT SF.[name]
    ,COALESCE(RIGHT([filename], NullIf(CHARINDEX(REVERSE('\'), REVERSE([filename])), 0)-1),
         [filename])as fileonly
    FROM 
    SYS.Databases S
    INNER JOIN sys.sysaltfiles SF
    ON S.[database_id] = SF.[dbid]
    WHERE  SF.groupid = 0 and SF.dbid = @dbid
    open cDBFiles

    FETCH NEXT FROM cDBFiles INTO @dbfilename,@dbfileonly


    WHILE (@@fetch_status <> -1)
    BEGIN
        -- Add file to restore script
        SET    @sqlrestore = @sqlrestore + 'MOVE ''' + @dbfilename + ''' to ''' 
        + @logfilelocal + '\' + @dbfileonly + ''''

        FETCH NEXT FROM cDBFiles INTO @dbfilename,@dbfileonly
        IF (@@fetch_status <> -1) 
            SET @sqlrestore = @sqlrestore + ', '
        Else
            SET @sqlrestore = @sqlrestore + ' '
    END
    close cDBFiles
    deallocate cDBFiles

    -- Add NORECOVERY
    SET @sqlrestore = @sqlrestore + ',NORECOVERY; '



	INSERT INTO @RestoreCmd(cmd_restore) VALUES (@sqlrestore)

    FETCH NEXT FROM cRestore INTO @dbid, @dbname
END

SELECT cmd_restore FROM @RestoreCmd

CLOSE cRestore
DEALLOCATE cRestore
