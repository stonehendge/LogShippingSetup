SELECT
   S.name [database_name]
   ,CASE WHEN lsd.primary_database is not null  THEN
		'LSP'
		WHEN lss.last_restored_date is not null THEN
		'LSS'
		WHEN (lsd.primary_database is null) and ( lss.last_restored_date is null) THEN
		'NONE'
    END [database_role]  
FROM 
    SYS.Databases S
	left join msdb.dbo.log_shipping_primary_databases lsd on lsd.primary_database=S.name
	left JOIN msdb.dbo.log_shipping_secondary_databases lss ON S.name=lss.secondary_database
WHERE S.database_id>4
ORDER BY [database_name]