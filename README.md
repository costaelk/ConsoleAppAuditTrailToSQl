# ConsoleAppAuditTrailToSQl #

Console App created to encode all the data present in a 
Siemens HMI Audit Trail file in a Sql Server table.

## Configuration ##

* First create a table in your database:

USE [YourDatabaseName]
GO

CREATE TABLE [dbo].[LOGAuditTrail](
	[RecordID] [bigint] NULL,
	[TimeStamp] [nvarchar](80) NOT NULL,
	[DeltaToUTC] [nvarchar](80) NULL,
	[UserID] [nvarchar](80) NULL,
	[ObjectID] [nvarchar](80) NULL,
	[Description] [nvarchar](255) NULL,
	[Comment] [nvarchar](135) NULL,
	[Checksum] [nvarchar](80) NOT NULL,
 CONSTRAINT [PK_LOGAuditTrail] PRIMARY KEY CLUSTERED 
(
	[TimeStamp] ASC,
	[Checksum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

* Now just run console app from cmd or script passing all necessary arguments.
  Here's an example:

  C:\Program Files\ConsoleAppAuditTrailToSQl.exe C:\HMISystem\Logs\LOGAuditTrail0.csv D:\Debug LOCALHOST\SQLEXPRESS sa 1234password YOURDATABASE

  PS:Add some Console.WriteLine(); more to facilitate debugging.


 * Enjoy the code!!!!!!!!!!

