--Create DATABASE
CREATE Database [DB_Oauth_API] 
GO

--Switch Database
USE [DB_Oauth_API]  
GO

--Create Login Table
CREATE TABLE [dbo].[Login]
(
    [id] [int] IDENTITY(1,1) NOT NULL,
    [username] [varchar](50) NOT NULL,
    [password] [varchar](50) NOT NULL,
    CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED   
(  
    [id] ASC  
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]  
GO
SET ANSI_PADDING OFF  
GO

--Insert data in login table
SET IDENTITY_INSERT [dbo].[Login] ON
INSERT [dbo].[Login]
    ([id], [username], [password])
VALUES
    (1, N'admin', N'adminpass')
SET IDENTITY_INSERT [dbo].[Login] OFF
SET ANSI_NULLS ON  
GO
SET QUOTED_IDENTIFIER ON  
GO

--Create stored procedure
CREATE PROCEDURE [dbo].[LoginByUsernamePassword]
    @username varchar(50),
    @password varchar(50)
AS
BEGIN
    SELECT id, username, password
    FROM Login
    WHERE username = @username
        AND password = @password
END  
  
GO 