USE [ToDoApp]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 07-09-2016 10:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ToDo]    Script Date: 07-09-2016 10:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToDo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Created] [date] NULL,
	[CatID] [int] NULL,
	[Finished] [bit] NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([ID], [Name]) VALUES (15, N'Mandag')
INSERT [dbo].[Category] ([ID], [Name]) VALUES (16, N'Tirsdag')
INSERT [dbo].[Category] ([ID], [Name]) VALUES (17, N'Onsdag')
INSERT [dbo].[Category] ([ID], [Name]) VALUES (18, N'Torsdag')
INSERT [dbo].[Category] ([ID], [Name]) VALUES (19, N'Fredag')
SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[ToDo] ON 

INSERT [dbo].[ToDo] ([ID], [Name], [Created], [CatID], [Finished]) VALUES (52, N'asdf', CAST(N'2016-09-06' AS Date), 15, 0)
INSERT [dbo].[ToDo] ([ID], [Name], [Created], [CatID], [Finished]) VALUES (54, N'45321', CAST(N'2016-09-06' AS Date), 15, 0)
INSERT [dbo].[ToDo] ([ID], [Name], [Created], [CatID], [Finished]) VALUES (55, N'asdf', CAST(N'2016-09-06' AS Date), 16, 1)
INSERT [dbo].[ToDo] ([ID], [Name], [Created], [CatID], [Finished]) VALUES (57, N'Møde', CAST(N'2016-09-07' AS Date), 19, 0)
SET IDENTITY_INSERT [dbo].[ToDo] OFF
ALTER TABLE [dbo].[ToDo]  WITH CHECK ADD  CONSTRAINT [FK_Task_Category] FOREIGN KEY([CatID])
REFERENCES [dbo].[Category] ([ID])
GO
ALTER TABLE [dbo].[ToDo] CHECK CONSTRAINT [FK_Task_Category]
GO
