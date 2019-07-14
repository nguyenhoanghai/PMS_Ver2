 
/****** Object:  Table [dbo].[P_Size]    Script Date: 25/06/2019 5:38:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[P_Size](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_P_Size] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_Size] ADD  CONSTRAINT [DF_P_Size_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

/****** Object:  Table [dbo].[P_Color]    Script Date: 25/06/2019 5:39:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[P_Color](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_P_Color] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_Color] ADD  CONSTRAINT [DF_P_Color_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO



ALTER TABLE [dbo].[Error]    ADD    [PhaseId] int NULL
GO
ALTER TABLE [dbo].[Error]  WITH CHECK ADD  CONSTRAINT [FK_Error_P_Phase] FOREIGN KEY([PhaseId])
REFERENCES [dbo].[P_Phase] ([Id])
GO

ALTER TABLE [dbo].[Error] CHECK CONSTRAINT [FK_Error_P_Phase]
GO

ALTER TABLE [dbo].[Error]    ALTER COLUMN    [Code] int NULL
GO
