USE [Analytics]
GO

/****** Object:  Table [dbo].[AN_ACe_STD]    Script) Date: 08/12/2014 14:17:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AN_ACe_STD]') AND type in (N'U'))
DROP TABLE [dbo].[AN_ACe_STD]
GO

CREATE TABLE [dbo].[AN_ACe_STD] (
     [ID] [int] IDENTITY(1,1) NOT NULL,
     [CalibrationDate] [DateTime] NULL,
     [Ident] varchar(150) null,
     [Dilution] money null,
     [F .Area] money null,
     [Acet .Area] money null,
     [Cl .Area] money null,
     [Br. Area] money null,
     [NO3.Area] money null,
     [Po4.Area] money null,
     [So4.Area] money null,
     [F PPM] money null,
     [Acet PPM] money null,
     [Cl PPM] money null,
     [Br PPM] money null,
     [No3 PPM] money null,
     [Po4 PPM] money null,
     [So4 PPM] money null,
CONSTRAINT [PK_AN_ACe_STD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
