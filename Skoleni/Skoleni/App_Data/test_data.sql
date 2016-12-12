
GO
SET IDENTITY_INSERT [dbo].[Authors] ON 

GO
INSERT [dbo].[Authors] ([AuthorId], [FirstName], [LastName]) VALUES (1, N'Miroslav', N'Holec')
GO
INSERT [dbo].[Authors] ([AuthorId], [FirstName], [LastName]) VALUES (3, N'James', N'Rollins')
GO
INSERT [dbo].[Authors] ([AuthorId], [FirstName], [LastName]) VALUES (6, N'Franz', N'Kafka')
GO
INSERT [dbo].[Authors] ([AuthorId], [FirstName], [LastName]) VALUES (7, N'Tom', N'Harper')
GO
SET IDENTITY_INSERT [dbo].[Authors] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

GO
INSERT [dbo].[Categories] ([CategoryId], [Title], [ParentCategoryId]) VALUES (3, N'Pohádky', NULL)
GO
INSERT [dbo].[Categories] ([CategoryId], [Title], [ParentCategoryId]) VALUES (4, N'Horory', NULL)
GO
INSERT [dbo].[Categories] ([CategoryId], [Title], [ParentCategoryId]) VALUES (7, N'Zlevněné knihy', NULL)
GO
INSERT [dbo].[Categories] ([CategoryId], [Title], [ParentCategoryId]) VALUES (8, N'Povídky', 7)
GO
INSERT [dbo].[Categories] ([CategoryId], [Title], [ParentCategoryId]) VALUES (9, N'Thrillery', NULL)
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
INSERT [dbo].[Books] ([BookId], [Title], [Description], [Price], [VatRate], [Perex], [Format], [Size], [Discriminator], [CategoryId]) VALUES (N'c1f3ba8b-e9b7-e611-af12-e4a471d99e1d', N'Amazonie', NULL, CAST(390.00 AS Decimal(18, 2)), CAST(0.15 AS Decimal(18, 2)), N'Kniha z amazonie', NULL, -4, N'Paperback', 9)
GO
INSERT [dbo].[Books] ([BookId], [Title], [Description], [Price], [VatRate], [Perex], [Format], [Size], [Discriminator], [CategoryId]) VALUES (N'7aa5d507-07b8-e611-af12-e4a471d99e1d', N'Odyssea', NULL, CAST(398.00 AS Decimal(18, 2)), CAST(0.15 AS Decimal(18, 2)), N'Novinka roku', NULL, -4, N'Paperback', 4)
GO
INSERT [dbo].[Books] ([BookId], [Title], [Description], [Price], [VatRate], [Perex], [Format], [Size], [Discriminator], [CategoryId]) VALUES (N'6684f691-3bb9-e611-af12-e4a471d99e1d', N'Povídka o černém notebooku', N'Nová knížka', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, -4, N'Paperback', 8)
GO
INSERT [dbo].[Books] ([BookId], [Title], [Description], [Price], [VatRate], [Perex], [Format], [Size], [Discriminator], [CategoryId]) VALUES (N'6784f691-3bb9-e611-af12-e4a471d99e1d', N'Humoreska', N'Trochu jiná kniha', CAST(50.00 AS Decimal(18, 2)), CAST(0.15 AS Decimal(18, 2)), NULL, NULL, -4, N'Paperback', 8)
GO
INSERT [dbo].[Books] ([BookId], [Title], [Description], [Price], [VatRate], [Perex], [Format], [Size], [Discriminator], [CategoryId]) VALUES (N'1745a709-cdbf-e611-af19-e4a471d99e1d', N'Štvanice', N'Nová kniha  Štvanice', CAST(499.00 AS Decimal(18, 2)), CAST(0.15 AS Decimal(18, 2)), NULL, -1, NULL, N'Ebook', 8)
GO
INSERT [dbo].[Books] ([BookId], [Title], [Description], [Price], [VatRate], [Perex], [Format], [Size], [Discriminator], [CategoryId]) VALUES (N'622cf619-cdbf-e611-af19-e4a471d99e1d', N'Štvanice', N'Nový paperback', CAST(399.00 AS Decimal(18, 2)), CAST(0.15 AS Decimal(18, 2)), NULL, NULL, -5, N'Paperback', 8)
GO
INSERT [dbo].[AuthorBooks] ([AuthorId], [BookId]) VALUES (3, N'c1f3ba8b-e9b7-e611-af12-e4a471d99e1d')
GO
INSERT [dbo].[AuthorBooks] ([AuthorId], [BookId]) VALUES (7, N'7aa5d507-07b8-e611-af12-e4a471d99e1d')
GO
INSERT [dbo].[AuthorBooks] ([AuthorId], [BookId]) VALUES (6, N'6684f691-3bb9-e611-af12-e4a471d99e1d')
GO
INSERT [dbo].[AuthorBooks] ([AuthorId], [BookId]) VALUES (7, N'6684f691-3bb9-e611-af12-e4a471d99e1d')
GO
INSERT [dbo].[AuthorBooks] ([AuthorId], [BookId]) VALUES (3, N'1745a709-cdbf-e611-af19-e4a471d99e1d')
GO
INSERT [dbo].[AuthorBooks] ([AuthorId], [BookId]) VALUES (3, N'622cf619-cdbf-e611-af19-e4a471d99e1d')
GO
SET IDENTITY_INSERT [dbo].[Comments] ON 

GO
INSERT [dbo].[Comments] ([CommentId], [AuthorId], [Text], [Subject]) VALUES (1, 1, N'Webový vývojář', NULL)
GO
INSERT [dbo].[Comments] ([CommentId], [AuthorId], [Text], [Subject]) VALUES (4, 1, N'Lenoch', NULL)
GO
INSERT [dbo].[Comments] ([CommentId], [AuthorId], [Text], [Subject]) VALUES (5, 3, N'Pravé jméno je James Czajkowski', 'Recenze')
GO
SET IDENTITY_INSERT [dbo].[Comments] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

GO
INSERT [dbo].[Users] ([UserId], [Username], [FavoriteCategories]) VALUES (1, N'mholec', N'1, 2, 3, 4, 5, 6, 7, 8, 9')
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
