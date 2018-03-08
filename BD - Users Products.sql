
IF OBJECT_ID('Prices', 'U') IS NOT NULL
DROP TABLE Prices
GO

IF OBJECT_ID('Products', 'U') IS NOT NULL
DROP TABLE Products
GO

IF OBJECT_ID('Locations', 'U') IS NOT NULL
DROP TABLE Locations
GO

IF OBJECT_ID('Users', 'U') IS NOT NULL
DROP TABLE Users
GO

-- Create the table in the specified schema
CREATE TABLE Users
(
   NameUser    [NVARCHAR](50)  PRIMARY KEY  NOT NULL,
   Password      [NVARCHAR](50)  NOT NULL,
   Email   [NVARCHAR](50) NOT NULL, 
   Role	INT NOT NULL,
   Name    [NVARCHAR](50)  NOT NULL,
   LastName    [NVARCHAR](50)  NOT NULL,
   DNI    [NVARCHAR](50) ,
   Score  INT NOT NULL,
   Enable bit NOT NULL,
   GUID [NVARCHAR](50) ,
);
GO
INSERT INTO Users
   ([NameUser],[Password],[Email],[Role],[Name],[LastName],[DNI],[Score],[Enable])
VALUES
   ( N'JavierR', N'35566D1B46F13B9BE91649964F6328', N'javier@ramos',1,N'Xavi', N'Ramos', N'37540073',100,1),
   ( N'PepeM', N'35566D1B46F13B9BE91649964F6328', N'pepe@martinez',1,N'Pepe', N'Martinez', N'64548073',100,1),
   ( N'MauricioM', N'35566D1B46F13B9BE91649964F6328', N'mauricio@macri',1,N'Mauricio', N'Macri', N'12740479',100,1),
   ( N'MartinaK', N'35566D1B46F13B9BE91649964F6328', N'martina@kors',2,N'Martina', N'Kors', N'93689562',100,1),
   ( N'DiegoM', N'35566D1B46F13B9BE91649964F6328', N'diego@maradona',4,N'Diego', N'Maradona', N'50440073',100,1),
   ( N'NestorK', N'35566D1B46F13B9BE91649964F6328', N'nestor@kirchner',4,N'Nestor', N'Kirchner', N'48547073',100,1),
   ( N'ErnestoG', N'35566D1B46F13B9BE91649964F6328', N'ernesto@guevara',3,N'Ernesto', N'Guevara', N'65740479',100,1),
   ( N'Juan', N'35566D1B46F13B9BE91649964F6328', N'juan@perez',1,N'Juan', N'Perez', N'96789562',100,1)
GO

-- Create the table in the specified schema
CREATE TABLE Products
(
   idProduct INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
   name nchar (25) NOT NULL,
   codeBar  nchar (15) NOT NULL,
   deleted  bit  NOT NULL,
);
GO

-- Create the table in the specified schema
CREATE TABLE Locations
(
   idLocation INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
   local  nchar (25) NOT NULL,
   address  nchar (80) NOT NULL,
   deleted  bit  NOT NULL,   
);
GO

-- Create the table in the specified schema
CREATE TABLE Prices
(
   idPrice INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
   value decimal (18, 2) NOT NULL,
   value_date datetimeoffset (7) NOT NULL,
   deleted bit NOT NULL,
   Qualification bit,
   idProduct INT NOT NULL FOREIGN KEY REFERENCES Products (idProduct),
   idLocation INT NOT NULL FOREIGN KEY REFERENCES Locations (idLocation), 
   NameUser [NVARCHAR](50) NOT NULL FOREIGN KEY REFERENCES Users (NameUser),
);

GO
INSERT INTO Products
   ([name],[codeBar],[deleted])
VALUES
   ( N'Pan', N'12346770986',0),
   ( N'Yogur', N'847592843659',0),
   ( N'Jugo', N'4875923449',0),
   ( N'Leche', N'4875923449',0)
GO
INSERT INTO Locations
   ([local],[address],[deleted])
VALUES
   ( N'Cotto', N'Av. Rivadavia 400',0),
   ( N'San Cayetano', N'Av. Corrientes 4800',0),
   ( N'Yaguar', N'Av. Avellaneda 300',0),
   ( N'Dia', N'Av. Corrientes 1800',0)
GO
INSERT INTO Prices
   ([value],[value_date],[idProduct],[idLocation],[deleted],[NameUser],[Qualification])
VALUES
   ( 30, N'1912-10-25 12:24:32.1277 +10:00',1,1,0, N'JavierR',0),
   ( 50, N'1912-10-25 12:24:32.1277 +10:00',2,2,0, N'JavierR',0),
   ( 10, N'1912-10-25 12:24:32.1277 +10:00',3,3,0, N'JavierR',0),
   ( 60, N'1912-10-25 12:24:32.1277 +10:00',4,4,0, N'JavierR',0)
GO

-- Consultas --

SELECT * FROM Users;
GO
SELECT Pr.idProduct, Pr.name, P.idPrice , P.value, L.idLocation,L.local, P.NameUser
from Prices P, Products Pr, Locations L where P.idProduct=Pr.idProduct and P.idLocation=L.idLocation;
GO
-- SELECT * from Prices;
-- SELECT * from Products;
-- SELECT * from Locations;