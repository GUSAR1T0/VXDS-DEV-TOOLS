INSERT INTO [authentication].[User] ([FirstName], [LastName], [Email], [Password], [Color], [Location], [Bio], [UserRoleId])
SELECT N'Roman', N'Mashenkin', N'xromash@vxdesign.store', N'vxdesign', N'rgba(8, 111, 114, 0.75)', N'Russia, Nizhny Novgorod', N'I''m developer', 1
UNION ALL
SELECT N'Anna', N'Boykova', N'aboykosha@mail.ru', N'11111111', N'rgba(11, 37, 142, 0.75)', N'Russia, Nizhny Novgorod', NULL, 2
UNION ALL
SELECT N'John', N'Smith', N'john.smith@mail.us', N'22222222', N'rgba(104, 170, 11, 0.75)', N'USA, CA, Los Angeles', NULL, 3;