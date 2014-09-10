CREATE TABLE [dbo].[PlanColumns] (
[ID] [int] IDENTITY(1,1) NOT NULL,
[InfoID] [int] NULL,
[Column1] varchar(50) null,
[Date&Time] varchar(50) null,
[OD] varchar(50) null,
[Plate] varchar(50) null,
[LC] varchar(50) null,
[GC] varchar(50) null,
[Scope] varchar(50) null,
[DCW] varchar(50) null,
[GlycerolStock] varchar(50) null,
[RateCheck] varchar(50) null,
[Total] varchar(50) null,
[Signoff] varchar(50) null,
[G2] varchar(50) null,
[Plate(attermination)] varchar(50) null,
[Plating] varchar(50) null,
[Citrate&Isocitrate] varchar(50) null,
[Viscosity] varchar(50) null,
[IC] varchar(50) null,
[OfflinepH] varchar(50) null,
[Time] varchar(50) null,
[NH4+/NH3] varchar(50) null,
[2ndLC] varchar(50) null,
[JRH] varchar(50) null,
[LC&YSI] varchar(50) null,
CONSTRAINT [PK_PlanColumns] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
