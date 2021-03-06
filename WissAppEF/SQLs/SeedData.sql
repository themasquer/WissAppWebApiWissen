USE [WissAppDB]
GO
SET IDENTITY_INSERT [dbo].[Messages] ON 

INSERT [dbo].[Messages] ([Id], [Message], [Date]) VALUES (1, N'Where is Fatih?', CAST(N'2020-02-25T15:41:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Messages] OFF
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [Name]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (2, N'User')
SET IDENTITY_INSERT [dbo].[Roles] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [RoleId], [UserName], [Password], [Email], [School], [Location], [BirthDate], [Gender], [IsActive], [IsDeleted]) VALUES (4, 1, N'fatih', N'fatih', N'fatih@wissen.com', N'Süleyman Demirel Üniversitesi', N'Konya', CAST(N'1996-02-02' AS Date), N'm', 1, 0)
INSERT [dbo].[Users] ([Id], [RoleId], [UserName], [Password], [Email], [School], [Location], [BirthDate], [Gender], [IsActive], [IsDeleted]) VALUES (5, 1, N'çağıl', N'çağıl', N'cagil@wissen.com', N'Bilkent Üniversitesi', N'İzmir', CAST(N'1980-03-03' AS Date), N'm', 1, 0)
INSERT [dbo].[Users] ([Id], [RoleId], [UserName], [Password], [Email], [School], [Location], [BirthDate], [Gender], [IsActive], [IsDeleted]) VALUES (6, 2, N'leo', N'oel', N'leo@wissen.com', NULL, N'Ankara', CAST(N'2014-03-03' AS Date), N'm', 1, 0)
SET IDENTITY_INSERT [dbo].[Users] OFF
SET IDENTITY_INSERT [dbo].[UsersMessages] ON 

INSERT [dbo].[UsersMessages] ([Id], [SenderId], [ReceiverId], [MessageId]) VALUES (1, 5, 4, 1)
SET IDENTITY_INSERT [dbo].[UsersMessages] OFF
