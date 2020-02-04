--BACKUP DATABASE [database_name_template] TO DISK = '" + item.src_share_path + "\\FULL_INIT_" + item.database_name + ".bak' WITH CHECKSUM, FORMAT, INIT, STATS = 10; ";
           