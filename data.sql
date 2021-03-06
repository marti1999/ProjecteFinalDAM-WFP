USE [RobaSegonaMa]
GO
SET IDENTITY_INSERT [dbo].[Status] ON 

INSERT [dbo].[Status] ([Id], [status], [reason]) VALUES (2, N'Activated', N'')
INSERT [dbo].[Status] ([Id], [status], [reason]) VALUES (3, N'Pending', N'')
INSERT [dbo].[Status] ([Id], [status], [reason]) VALUES (4, N'Declined', N'Because your salary is too high')
INSERT [dbo].[Status] ([Id], [status], [reason]) VALUES (5, N'Declined', N'Rao2')
SET IDENTITY_INSERT [dbo].[Status] OFF
SET IDENTITY_INSERT [dbo].[MaxClaims] ON 

INSERT [dbo].[MaxClaims] ([Id], [value], [operationResult]) VALUES (1, 30, 500)
SET IDENTITY_INSERT [dbo].[MaxClaims] OFF
SET IDENTITY_INSERT [dbo].[Language] ON 

INSERT [dbo].[Language] ([Id], [code], [name]) VALUES (1, N'en', N'English')
INSERT [dbo].[Language] ([Id], [code], [name]) VALUES (3, N'ca', N'Català')
SET IDENTITY_INSERT [dbo].[Language] OFF
SET IDENTITY_INSERT [dbo].[Requestor] ON 

INSERT [dbo].[Requestor] ([dni], [name], [lastName], [birthDate], [gender], [password], [email], [securityAnswer], [securityQuestion], [dateCreated], [active], [householdIncome], [householdMembers], [picturePath], [Id], [Language_Id], [MaxClaims_Id], [Status_Id]) VALUES (N'5845684F', N'Requester', N'test', CAST(N'1998-01-01T00:00:00.000' AS DateTime), N'M', N'1234', N'requester@requester.com', N'a', N'a', CAST(N'2018-01-01T00:00:00.000' AS DateTime), 1, 800, 3, N'/', 2, 1, 1, 3)
INSERT [dbo].[Requestor] ([dni], [name], [lastName], [birthDate], [gender], [password], [email], [securityAnswer], [securityQuestion], [dateCreated], [active], [householdIncome], [householdMembers], [picturePath], [Id], [Language_Id], [MaxClaims_Id], [Status_Id]) VALUES (N'5865844F', N'Requester2', N'test2', CAST(N'1998-01-01T00:00:00.000' AS DateTime), N'F', N'1234', N'requester@requester.com', N'a', N'a', CAST(N'2019-01-01T00:00:00.000' AS DateTime), 1, 1000, 1, N'/', 3, 1, 1, 3)
SET IDENTITY_INSERT [dbo].[Requestor] OFF
SET IDENTITY_INSERT [dbo].[Warehouse] ON 

INSERT [dbo].[Warehouse] ([Id], [street], [number], [city], [postalCode], [name]) VALUES (1, N'Roger de Flor', N'25', N'Granollers', N'08401', N'Magatzem de Roba')
SET IDENTITY_INSERT [dbo].[Warehouse] OFF
SET IDENTITY_INSERT [dbo].[Size] ON 

INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (1, N'XS', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (2, N'S', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (3, N'M', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (4, N'L', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (5, N'XL', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (6, N'XXL', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (7, N'XXXL', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (8, N'38', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (9, N'39', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (10, N'40', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (11, N'41', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (12, N'42', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (13, N'43', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (14, N'44', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (15, N'45', 1)
INSERT [dbo].[Size] ([Id], [size], [active]) VALUES (16, N'46', 1)
SET IDENTITY_INSERT [dbo].[Size] OFF
SET IDENTITY_INSERT [dbo].[Gender] ON 

INSERT [dbo].[Gender] ([Id], [gender], [active]) VALUES (1, N'male', 1)
INSERT [dbo].[Gender] ([Id], [gender], [active]) VALUES (2, N'female', 1)
INSERT [dbo].[Gender] ([Id], [gender], [active]) VALUES (3, N'Other', 1)
SET IDENTITY_INSERT [dbo].[Gender] OFF
SET IDENTITY_INSERT [dbo].[Color] ON 

INSERT [dbo].[Color] ([Id], [colorCode], [name], [active]) VALUES (7, N'#008000', N'green', 1)
INSERT [dbo].[Color] ([Id], [colorCode], [name], [active]) VALUES (9, N'#FF00FF', N'fuchsia', 1)
INSERT [dbo].[Color] ([Id], [colorCode], [name], [active]) VALUES (13, N'#FFFFFF', N'white', 1)
INSERT [dbo].[Color] ([Id], [colorCode], [name], [active]) VALUES (14, N'#000000', N'black', 1)
INSERT [dbo].[Color] ([Id], [colorCode], [name], [active]) VALUES (15, N'#808080', N'gray', 1)
INSERT [dbo].[Color] ([Id], [colorCode], [name], [active]) VALUES (16, N'#FF0000', N'red', 1)
INSERT [dbo].[Color] ([Id], [colorCode], [name], [active]) VALUES (17, N'#800000', N'brown', 1)
INSERT [dbo].[Color] ([Id], [colorCode], [name], [active]) VALUES (18, N'#FFFF00', N'yellow', 1)
INSERT [dbo].[Color] ([Id], [colorCode], [name], [active]) VALUES (19, N'#0000FF', N'blue', 1)
INSERT [dbo].[Color] ([Id], [colorCode], [name], [active]) VALUES (20, N'#800080', N'purple', 1)
SET IDENTITY_INSERT [dbo].[Color] OFF
SET IDENTITY_INSERT [dbo].[Classification] ON 

INSERT [dbo].[Classification] ([Id], [classificationType], [active], [value]) VALUES (1, N'Trousers', 1, 2)
INSERT [dbo].[Classification] ([Id], [classificationType], [active], [value]) VALUES (2, N'Jeans', 1, 2)
INSERT [dbo].[Classification] ([Id], [classificationType], [active], [value]) VALUES (3, N'Shirt', 1, 1)
INSERT [dbo].[Classification] ([Id], [classificationType], [active], [value]) VALUES (4, N'T-shirt', 1, 1)
INSERT [dbo].[Classification] ([Id], [classificationType], [active], [value]) VALUES (5, N'Tracksuit', 1, 1)
INSERT [dbo].[Classification] ([Id], [classificationType], [active], [value]) VALUES (6, N'Sweatpants', 1, 2)
INSERT [dbo].[Classification] ([Id], [classificationType], [active], [value]) VALUES (7, N'Shoes', 1, 3)
INSERT [dbo].[Classification] ([Id], [classificationType], [active], [value]) VALUES (8, N'Jacket', 1, 3)
INSERT [dbo].[Classification] ([Id], [classificationType], [active], [value]) VALUES (9, N'Scarf', 1, 1)
INSERT [dbo].[Classification] ([Id], [classificationType], [active], [value]) VALUES (10, N'Gloves', 1, 2)
SET IDENTITY_INSERT [dbo].[Classification] OFF
SET IDENTITY_INSERT [dbo].[Clothes] ON 

INSERT [dbo].[Clothes] ([Id], [dateCreated], [active], [Warehouse_Id], [Size_Id], [Color_Id], [Gender_Id], [Classification_Id]) VALUES (4, CAST(N'2019-02-02T00:00:00.000' AS DateTime), 1, 1, 1, 9, 1, 1)
SET IDENTITY_INSERT [dbo].[Clothes] OFF
SET IDENTITY_INSERT [dbo].[Donor] ON 

INSERT [dbo].[Donor] ([Id], [name], [lastName], [birthDate], [gender], [password], [email], [securityAnswer], [securityQuestion], [dateCreated], [active], [picturePath], [ammountGiven], [dni], [points], [Language_Id]) VALUES (2, N'Martí', N'Caixal', CAST(N'1999-06-07T00:00:00.000' AS DateTime), N'male', N'1234', N'marti@gmail.com', N'nom_', N'marti', CAST(N'2019-02-25T00:00:00.000' AS DateTime), 1, NULL, 0, N'45698745H', 0, 1)
INSERT [dbo].[Donor] ([Id], [name], [lastName], [birthDate], [gender], [password], [email], [securityAnswer], [securityQuestion], [dateCreated], [active], [picturePath], [ammountGiven], [dni], [points], [Language_Id]) VALUES (6, N'Víctor', N'Auyanet', CAST(N'1999-06-07T00:00:00.000' AS DateTime), N'male', N'1234', N'victor@gmail.com', N'nom?', N'victor', CAST(N'1999-06-07T00:00:00.000' AS DateTime), 1, NULL, 0, N'47896541F', 0, 3)
SET IDENTITY_INSERT [dbo].[Donor] OFF
SET IDENTITY_INSERT [dbo].[Names] ON 

INSERT [dbo].[Names] ([Id], [itemId], [nameInLanguage], [itemType], [Language_Id]) VALUES (1, 1, N'Pantaló', N'Classification', 3)
SET IDENTITY_INSERT [dbo].[Names] OFF
SET IDENTITY_INSERT [dbo].[Administrator] ON 

INSERT [dbo].[Administrator] ([Id], [email], [name], [lastName], [password], [dateCreated], [isSuper], [active], [workerCode], [Language_Id], [Warehouse_Id]) VALUES (1, N'a', N'a', N'a', N'a', CAST(N'2019-02-08T00:00:00.000' AS DateTime), 1, 1, N'354756436a', 1, 1)
SET IDENTITY_INSERT [dbo].[Administrator] OFF
SET IDENTITY_INSERT [dbo].[Announcement] ON 

INSERT [dbo].[Announcement] ([Id], [title], [message], [dateCreated], [language], [recipient]) VALUES (2, N'asdf', N'asdfasfasdf', CAST(N'2019-02-11T18:32:57.900' AS DateTime), N'System.Windows.Controls.ComboBoxItem: CAT', N'Requestors')
INSERT [dbo].[Announcement] ([Id], [title], [message], [dateCreated], [language], [recipient]) VALUES (3, N'2', N'missatge 2 a tothom', CAST(N'2019-02-15T16:37:50.573' AS DateTime), N'System.Windows.Controls.ComboBoxItem: CAT', N'Requestors')
INSERT [dbo].[Announcement] ([Id], [title], [message], [dateCreated], [language], [recipient]) VALUES (4, N'new announcement', N'thi sis a new announcement
', CAST(N'2019-03-07T19:11:09.190' AS DateTime), N'System.Windows.Controls.ComboBoxItem: CAT', N'Requestors')
SET IDENTITY_INSERT [dbo].[Announcement] OFF
