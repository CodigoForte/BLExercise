USE [Master]

IF EXISTS(select * from sys.databases where name='<DB>')
	ALTER DATABASE [<DB>] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

IF EXISTS(select * from sys.databases where name='<DB>')
	DROP DATABASE [<DB>] 