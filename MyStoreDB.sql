USE a03
GO

DROP DATABASE MyStore
GO

CREATE DATABASE MyStore 
GO     

ALTER DATABASE MyStore
COLLATE Cyrillic_General_CI_AS	
GO

USE MyStore
GO
--1***************************
CREATE TABLE MyStore.dbo.Employees (
  EmployeeId int NOT NULL IDENTITY CONSTRAINT PK_Employees_EmployeeId PRIMARY KEY,
  Name nvarchar(50) NOT NULL,
  SecondName nvarchar(50) NOT NULL,
  Email nvarchar(50) NOT NULL UNIQUE,
  PassHash nvarchar(50) NOT NULL
);
GO

INSERT MyStore.dbo.Employees(Name, SecondName, PassHash, Email) VALUES ('-', '-', '-', N'-@-')
INSERT MyStore.dbo.Employees(Name, SecondName, PassHash, Email) VALUES ('�������', '������', 'a', N'a@a' )
INSERT MyStore.dbo.Employees(Name, SecondName, PassHash, Email) VALUES ('����', '������', 'b', N'b@b')
GO


--2**********************************
CREATE TABLE MyStore.dbo.Clients (
  ClientId int NOT NULL IDENTITY CONSTRAINT PK_Clients_ClientId PRIMARY KEY,
  RegisterDate datetime NOT NULL,
  Email nvarchar(50) NOT NULL UNIQUE,
  PassHash nvarchar(50) NOT NULL,
  Phone bigint NOT NULL,
  DeliveryMeth nvarchar(50) NULL,
  Name nvarchar(50) NOT NULL,
  SecondName nvarchar(50) NULL,
  City nvarchar(50) NULL,
  Street nvarchar(50) NULL,
  House nvarchar(50) NULL,
  Apartament nvarchar(50) NULL,
  UkrIndex int NULL,
  NPNumber int NULL
)
GO

INSERT MyStore.dbo.Clients(RegisterDate, Email, PassHash, Phone, DeliveryMeth, Name, SecondName, City, Street, House, Apartament, UkrIndex, NPNumber) VALUES
( '20211110', N'ligula.nullam@aol.org', 'p', '14434757694', '�ourier ', '��������', '�����', '���������', '����������', N'10', N'86', 5464, 45),
( '20210913', N'dis@google.org', 'p', '18746182285', 'UkrPost', '������', '�����', '����', '����������', N'5', N'65', 3412, 21),
( '20201225', N'orci.ut@yahoo.ca', 'p', '12227435874', 'Npost', '��������', '����', '����', '�������', N'9', N'76', 1113, NULL),
( '20190512', N'nunc@protonmail.org', 'p', '17229072538', '�ourier ', '����������', '�������', '�����', '������������', N'14', N'21', 3113, 44),
( '20190512', N'dolor.sit@hotmail.org', 'p', '12473165122', '�ourier ', '�������', '�������', '������', '������������', N'27', N'78', NULL, 13)
GO


--3**********************************
CREATE TABLE MyStore.dbo.Characteristics (
  CharacteristicId int NOT NULL IDENTITY CONSTRAINT PK_DetailsKeys_DetailKeyId PRIMARY KEY,
  CharacteristicName nvarchar(50) NULL UNIQUE --���� �������������� � ������������ �� ����������, �� � ����������� ������ ����� � �� ���� ��������������, �� ����� NULL

)
GO

INSERT MyStore.dbo.Characteristics(CharacteristicName) VALUES
('����������� �����������'),--1
('������ �������'),         --2
('��� �������'),            --3
('������� ������'),         --4
('���������� �� ������ ���������'),--5
('���������� ���������'),          --6
('����������� ��������� �� �����'),--7
('���������� �������'),          --8
('���� ������'),                 --9
('��� �������� ������'),         --10
('��� �������� �������'),        --11
('��� ������'),                  --12
('������������ �������� ������'),--13
('���������� �����������'),      --14
('������������ ���������� �������� �������'),--15
('������� ������������� ��������'),          --16
('������ ���������'),            --17
('������ �� ������'),            --18
('������������ ���'),            --19
('���� �� �����������'),         --20
('���� �� ���������'),           --21
('-')                            --22
GO

--CREATE INDEX index_Characteristics_Characteristic
--  ON Characteristics (Characteristic)
--GO

  --4**********************************  
CREATE TABLE MyStore.dbo.Categories (
  CategoryId int NOT NULL IDENTITY CONSTRAINT PK_Categories_CategoryId PRIMARY KEY,
  CategoryName nvarchar(50) NOT NULL
)
GO

INSERT MyStore.dbo.Categories(CategoryName) VALUES
('Cameras'),--1
('VideoRecorders'),--2
('MovementSensors'),--3
('Switches'),--4
('HDDs'),--5
('KomplektyDomofonov'),--6
('KontrollerySKD'),--7
('SchityvateliKartDostupa'),--8
('KomplektySignalizatsiy'),--9
('Tsentrali')--10
GO


--5**********************************
 CREATE TABLE MyStore.dbo.[Values] (
  ValueId int NOT NULL IDENTITY CONSTRAINT PK_Values_ValueId PRIMARY KEY,
  --CategoryId int NOT NULL CONSTRAINT FK_DetailsValues_Categories FOREIGN KEY (CategoryId)  REFERENCES Categories (CategoryId),
  --DetailKeyId int NOT NULL CONSTRAINT FK_DetailsValues_DetailKeys FOREIGN KEY (DetailKeyId)  REFERENCES DetailKey (DetailKeyId),
  ValueName nvarchar(100) NOT NULL UNIQUE
)
GO

INSERT MyStore.dbo.[Values](ValueName) VALUES
('2 Mp (1980p)'),--1
('4 Mp 2688x1520'),--2
('1 Mp (720p)'),--3
('1/3"'),--4
('1/4"'),--5
('CMOS'),--6
('OmniVision OV9712'),--7
('Auto'),--8
('24 ��'),--9
('20 �'),--10
('����'),--11
('12 �'),--12
('IP ONVIF'),--13
('4 BNC'),--14
('SATA HDD'),--15
('�� '),--16
('�����������������'),--17
('14 �'),--18
('2000 �'),--19
('1,8� 2,4 �, ������������� 2,2 �'),--20
('3�'),--21
('8��/18�� (� �����������/���������� �����������)'),--22
('90 ����'),--23
('85 ����'),--24
('88,5 ����'),--25
('57 ����'),--26
('���'),--27
('AHD'),--28
('5 �'),--29
('--')
GO


--6**********************************
CREATE TABLE MyStore.dbo.CategoryCharacteristics (
  CategoryCharacteristicsId int NOT NULL IDENTITY CONSTRAINT PK_CategoryCharacteristics_CharacteristicValuesId PRIMARY KEY,
  CategoryId int NOT NULL        CONSTRAINT FK_CategoryCharacteristics_Categories FOREIGN KEY (CategoryId)  REFERENCES Categories (CategoryId),
  CharacteristicId INT NOT NULL  CONSTRAINT FK_CategoryCharacteristics_Characteristics FOREIGN KEY (CharacteristicId)  REFERENCES Characteristics (CharacteristicId),
  OrdinationNumber INT NOT NULL
)
GO

INSERT MyStore.dbo.CategoryCharacteristics(CategoryId, CharacteristicId, OrdinationNumber) VALUES
( 1, 1, 1),--1     ����������� �����������
( 1, 2, 2),--2     ������ �������
( 1, 3, 3),--3     ��� �������
( 1, 4, 4),--4     ������� ������
( 1, 5, 5),--5     ���������� �� ������ ���������
( 1, 6, 6),--6     ���������� ���������
( 1, 7, 7),--7     ����������� ��������� �� �����
( 1, 8, 8),--8     ���������� �������
( 1, 9, 9),--9     ���� ������
( 1, 13, 10),--10  ������������ �������� ������
( 1, 10, 11),--11  ��� �������� ������
( 2, 11, 1),--12   ��� �������� �������
( 2, 8,  2), --13  ���������� �������
( 2, 12, 3),--14   ��� ������
( 3, 13, 1),--15   ������������ �������� ������
( 3, 14, 2),--16   ���������� �����������
( 3, 15, 3),--17   ������������ ���������� �������� �������
( 3, 16, 4),--18   ������� ������������� ��������
( 3, 17, 5),--19   ������ ���������
( 3, 18, 6),--20   ������ �� ������
( 3, 19, 7),--21   ������������ ���
( 3, 20, 8),--22   ���� �� �����������
( 3, 21, 9),--23   ���� �� ���������
( 3, 8,  10)--24   ���������� �������
GO

--7**********************************
CREATE TABLE MyStore.dbo.CharacteristicValues (
  CharacteristicValuesId int NOT NULL IDENTITY CONSTRAINT PK_CharacteristicValues_CharacteristicValuesId PRIMARY KEY,
   
  CategoryCharacteristicsId int NOT NULL CONSTRAINT FK_CharacteristicValues_CategoryCharacteristics FOREIGN KEY (CategoryCharacteristicsId)  REFERENCES CategoryCharacteristics (CategoryCharacteristicsId),
  ValueId int NOT NULL CONSTRAINT FK_CharacteristicValues_Values FOREIGN KEY (ValueId)  REFERENCES [Values] (ValueId)
)
GO

INSERT MyStore.dbo.CharacteristicValues(CategoryCharacteristicsId, ValueId) VALUES 
(1, 1),--    ����������� �����������         2 Mp (1980p)
(1, 2),--    ����������� �����������         4 Mp 2688x1520
(1, 3),--    ����������� �����������         1 Mp (720p)
(2, 4),--    ������ �������                  1/3"
(2, 5),--    ������ �������                  1/4"
(3, 6),--    ��� �������--                   CMOS
(3, 7),--    ��� �������                     OmniVision OV9712
(4, 8),--    ������� ������                  Auto
(5, 9),--    ���������� �� ������ ���������  24 ��
(6, 10),--   ���������� ���������            20 �
(6, 18),--   ���������� ���������            14 � 
(7, 16),--   ����������� ��������� �� �����  ��
(7, 27),--   ����������� ��������� �� �����  ���
(8, 12),--   ���������� �������              12 �
(9, 24),--   ���� ������                     85 ����
(9, 26),--   ���� ������                     57 ����
(10, 11),--  ������������ �������� ������    ����
(10, 27),--  ������������ �������� ������    ���
(11, 13),--  ��� �������� ������             IP ONVIF
(11, 28),--  ��� �������� ������             AHD
(12, 14),--  ��� �������� �������            4 BNC
(12, 13),--  ��� �������� �������            IP ONVIF
(13, 12),--  ���������� �������              12 �
(14, 28),--  ��� ������                      AHD 
(15, 11),--  ������������ �������� ������    ����
(15, 27),--  ������������ �������� ������    ���
(16, 18),--  ���������� �����������          14 �
(17, 19),--  ������������ ���������� �������� �������  2000 �
(18, 16),--  ������� ������������� ��������  ��
(19, 20),--  ������ ���������                1,8� 2,4 �, ������������� 2,2 �
(20, 11),--  ������ �� ������                ����
(21, 22),--  ������������ ���                8 ��/18 �� (� �����������/���������� �����������)
(22, 23),--  ���� �� �����������             90 ����
(22, 25),--  ���� �� �����������             88,5 ����
(23, 23),--  ���� �� ���������               90 ����
(24, 21), --  ���������� �������              3�
(15, 30),(16, 30),(17, 30),(18, 30),(20, 30),(23, 30),(24, 30),
(19, 30),(21, 30),(22, 30),(12, 30),(13, 30),(14, 30),(1, 30),
(2, 30),(3, 30),(4, 30),(5, 30),(6, 30),(7, 30),(8, 30),(10, 30),
(11, 30)
GO


--DROP TABLE MyStore.dbo.Colors
  --8**********************************
CREATE TABLE MyStore.dbo.Colors (
  ColorId int NOT NULL IDENTITY CONSTRAINT PK_Colors_ColorId PRIMARY KEY,
  ColorName NVARCHAR(50) NULL
)
GO

INSERT MyStore.dbo.Colors(ColorName) VALUES
('White'),
('Black'),
('Gray'),
  ('-')
GO


  --DROP TABLE MyStore.dbo.Sections
--9**********************************
CREATE TABLE MyStore.dbo.Sections (
  SectionId int NOT NULL IDENTITY CONSTRAINT PK_Sections_SectionId PRIMARY KEY,
  SectionName NVARCHAR(50) NOT NULL
)
GO

INSERT MyStore.dbo.Sections(SectionName) VALUES
('VideoObservation'),
('AccessControl'),
('Alarms')
GO


--drop TABLE MyStore.dbo.Products
--10**********************************
CREATE TABLE MyStore.dbo.Products (
  ProductId int NOT NULL IDENTITY CONSTRAINT PK_Products_ProductId PRIMARY KEY,
  CategoryId int NOT NULL --CONSTRAINT FK_Products_CategoryCharacteristics FOREIGN KEY (CategoryId)  REFERENCES CategoryCharacteristics (CategoryId),--��� �� Categories 
                          CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId)  REFERENCES Categories (CategoryId),
  SectionId INT NOT NULL CONSTRAINT FK_Products_Sections FOREIGN KEY (SectionId)  REFERENCES Sections (SectionId),
  Description nvarchar(4000) NULL
)
GO

INSERT MyStore.dbo.Products(CategoryId, SectionId, Description) VALUES
(1, 1, '������� ����������� ����� AHD �-1� - ��������� ������, ���������� �� ���������� AHD ��������������� ��� ��������������� � ������ ��������� ������� �� ����� ��������. ������ ������ �������� � ������ ������������ ������ � ������� �������, ������� ������������ �������� �������� ���������� AHD 720p. ��� ����� ���� � ��������� �����������������, �������������� ������������� ������ � AHD � IP ��������. �������� ������� �� ���������� ��������: ������ ������������ � ��������� ������� � ����������������� ��� ������ ��������. ������ ����������� ���������� �����, � �� ������� �������� ����� �� ���������������� ��� ����� ������������.'),
(1, 1, '������� ����������� Tecsar AHDW-1Mp-20Fl-eco (����� ����������� ������ ������� Tecsar AHDW-1M-20F-eco � ����������� ����������������) - ��������� ������, ���������� �� ���������� AHD ��������������� ��� ��������������� � ������ ��������� ������� �� ����� ��������. ������ ������ �������� � ������ ������������ ������ � ������� �������, ������� ������������ �������� �������� ���������� AHD 720p. ��� ����� ���� � ��������� �����������������, �������������� ������������� ������ � AHD � IP ��������. �������� ������� �� ���������� ��������: ������ ������������ � ��������� ������� � ����������������� ��� ������ ��������. ������ ����������� ���������� �����, � �� ������� �������� ����� �� ���������������� ��� ����� ������������.'),
(1, 1, 'Tecsar AHDW-1Mp-40Vfl - ������������������� ���������� ������� ������. ����� ������� ���������� ������ Tecsar AHDW-1M-40V � ������������ ����������������. �������� ��� ����� ��������. ������������ ���������� ������ ������������ ����������� ������������ ����� ��������� ����������� �������. ����������� � ������� �������������� ���������������� ����������� � ���������� � ��������� AHD. ������������ �������� ���������� ��������� ������������� �������� ���� ������ �� ��������, ��� � �������� "������" �� ������, ������������ "����������" �������� � ��������� ���. ����������� ����������� ����� AHD �, Tecsar AHDW-1Mp-40Vfl � ��� �����, ������� ����������� ����������� ���������� ��� ������� ������� ���������� ���������, �� � �� �� ����� ��� �� ������� �������� � �� ������� ����������� ������������, ��� IP ������. ��� ������ ��������� ������ ���������������� ��� ����� �������, � ���������� ������ AHD. ��� ����� ���� � ��������� �����������������, �������������� ������ � AHD, IP � �������� ����������� ��������. �������� ������� �� ���������� ��������: ������ ������������ � ��������� ������� � ����������������� ��� ������ ��������. ������ ������� ���������� �����, � �������� ����� �� ���������������� ��� ����� ������������. '),
(1, 1, 'IP ������ ������ ���� � ������������ ����������� 4�� 2688x1520 ����� ��� ������� ������ 20 �/�
������� ������ ����� ��� ���� ����������. ����� ������������� ���������� ����� �� ���������� ������ 5�� � ����������� ������� � ������ ������ �����������, � �� 3�� (2048 x 1536) �������� ����������� �� ����������� ��� � ����������� ���������� ��������� ������������ ������������ ����� ���������.
����� ���������������� ������� WDR 120�� ������������ ������������� ������������ ������������� ������������� ������� ����������� � �������������� ������ �����. ������ ��� WDR ������ "��������" ����� ������ ������� ��� ������� ��� ������ ������ ������� � ����. wdr ����������� �������� �������� (�� ������� �������), � ����������� hikvision DS-2CD2042WD-I � WDR 120dB  ������� ����������� �� ����������, ���������� ������������ �� ������� ��������.'),
(1, 1, '������� � �����������
������ ������������� �� ����� ������������ ������������ Tecsar - �������� �������������� NVR, ���������� ������������������� HVR AHD ���������������, ���������� ������������������� CVBS
������ �������� �������������� �������� 1/4" Omnivision 2 Megapixel CMOS Sensor
��������� HiSilicon HI3518C, ������������ �������� ����� ������ 720p ��� ������� ������ 25 �/�, ������������ ��� ������� ��� ��������� ������������ ����������� � ����� �������� ������
��������� �� ������������ 5-�� ����������� ��������� ������ �� ���������� 20 ������ � ������ �������
���������� � ������������� ������� �� �������� ������ IP66
��������� ���������� PoE (Power over Ethernet), ����������� ������ ���������� � ���������� ����������� �� ������ ������ "����� ����"
������ ����� ������ ���������������� ����������� web ���������? ���������� �������, ����� ������� �������������� ������ �� ���� �������� � ����������.
��������� ���������� ���������� AES, DES, 3DES'),
(2, 1, '4-� ��������� ���������������� � ���������� 3-� ����� �����: ������� ���������� (CVBS), AHD 1 � 1,3 ��, IP. �������������� ������ - 4 ������ �����, �������������� ���������� ���������� HDMI, ��������� ���������� ����������������� �������� (PTZ). ��������� ������ ����� �� ��������� �����������, ������ �������������� ������. '),
(2, 1, '����������� ��������� ���������������� � ������������ �������. ������������ ��� ������ � ��������� �������� ���������������, � ����� ����������� �����, ������������ �� 256 ������������������. ������������ �� ���� ������������������ ���������, ��������������� ��� �������������� ������ � ��������������� ����� � ����� ����������. ���������� ��������� ������������ � AHD ��� ���������� ����� ����� 4 BNC �������, ����������� � IP ����� - ����� ������� ������ LAN. ��� ������ � ����� ������������ ������������ ������� ��� ��������� � �������� VGA. ����� ������������ � ���������. ���������������� Tecsar HDVR Neo-Futurist ��������� ������������� ����� � �������� �������, �������������� ����������, �������� ������ ������ ��� ��������� � �������� � ������ ���������� ��� ���������� ���������� �� ���� ��������. ������������ ���������� ������ � ��������� ������� tecsar-cloud, ������� ��������� �������� ��� �������� IP ������ � ����� ��������� ������������. �������� ������ �� �������� � ������������ ����������������, � ��� ����� ����� ��������� ������ ��� �������� ������ � �������� ������ �����.'),
(2, 1, '������ ���������������� � ������������ �������, ��������������� ��� ������������ ������ ��������������� ��� ����������� ������� �� ����� ��������. ��������� ����������� � ���������� �/��� �������� ����� �� ��������. �������� ����������,  ������������ �������� � ���������� ����������� �� ������ � ���������� � IP �����. ������� ����������� �� ������� � VGA �������� ��� ������� ���������. ������ ��������� ����: �������� ���� ����������, ������ � ��������� �������, ���������������� ���������. ������������ ���� ������� ���������� ��� ��������� ���������������.'),
(3, 3,'����� ������� ������������ ������ ��������: ��������� ����������������� ������ ��� ���������� �� ����� ������������ ����������� ������������ ������ � ����������� ����������.
�������������� - ����������� �������� ��������
������� ������� ����������������� ��������
������� ���������������
�������������� ����������� ������������� ���������
������������ � ������ ��� ������ ������������ �� -10�
���������� �� ����������� ������� 18 � ��� �������������� ���� ������ 90�
����������� ��������� ������� ��� ������ 
��������������� ����
������ �������� �� ��������, ������������� ��������'),
(3, 3,'����� ������� ������������ ������ ��������: ��������� ����������������� ������ ��� ���������� �� ����� � ����������� ����� ������������ ����������� ������������ ������ � ����������� ����������.
�������������� - ����������� �������� ��������
������ �� ������������ �� ��������
������� ������� ����������������� ��������
������� ���������������
�������������� ����������� ������������� ���������
������������ � ������ ��� ������ ������������ �� -10�
���������� �� ����������� ������� 18 � ��� �������������� ���� ������ 90�
����������� ��������� ������� ��� ������ 
��������������� ����
������ �������� �� ��������, ������������� ��������'),
(3, 3,'������ �������� ��� ��������: ��������� ������ ��� ������� ���������, ������� ��������� �� ������� ���������� ������ ��������� � �� ����������� �� �������� �������� �� 25 ��. ������ ��������� ������������ �������� ����������� ���������.
 
������������ ���������� �� �������� �� 100 ������
������� �� ���� ����������� �������� ���� �AA� �� 12 �������
������� �� ������ ������������
��������� ����������� �������� �������� 12 ������
����������� ����������������
������������� ����� � ����� ����� �� 25 ���������
�������������� ���� ������ - 110�
�������� ��������� ������ �� ������� ������ ����������  433 ���, ������� �� ������� ����������� ��������
�������� �� ����� ������������ ������� ������: ECONOM, COMFORT, MULTIZONE, MULTIZONE II, CYCLOP II'),
(3, 3, '����������� ������ ��������: ������ ����������, �������� ������, ��������� � Ajax ocBridge.
 
������������ ���������� �� �������� �� 2000 ������
���������� ����� � ���������
������������ ������ ������ ��������
������������ ���������������� ������
������ �� ��������� �� 7 ���
����������� �������� ������������� ��������
��������� ����������� 14 ������
����������� ����������������
������������� �������� �������� ����� �� 20-�� ��, ������� �� 50 ��
�������������� ���� ������ - 88,5�
���������� ������� ������ ����������  868 ���, ������� �� ������� ����������� ��������
���������� � ������� ���������, ������������ �������, ������� �������� ������������')
GO

--DROP TABLE MyStore.dbo.ColoredProducts
--11**********************************
CREATE TABLE MyStore.dbo.ColoredProducts (
  ProductId int NOT NULL CONSTRAINT FK_ColoredProducts_Products FOREIGN KEY (ProductId)  REFERENCES Products (ProductId),
  ProductCode nvarchar(200) NULL UNIQUE,
  ProductName nvarchar(200) NULL,
  Price decimal NULL,
  ImgResized VARBINARY(MAX) NULL,
  ColorId int NOT NULL CONSTRAINT FK_ColoredProducts_Colors FOREIGN KEY (ColorId)  REFERENCES Colors (ColorId),
                   --CONSTRAINT FK_ColoredProducts_Products FOREIGN KEY (ColorId)  REFERENCES ProductsCatDescr (ColorId),
  Stock int NOT NULL,
  ShowProduct BIT NOT NULL

  CONSTRAINT PK_ColoredProducts PRIMARY KEY (ProductId, ColorId)
)
GO

INSERT MyStore.dbo.ColoredProducts(ProductId, ProductCode, ProductName, Price, ImgResized, ColorId, Stock, ShowProduct) VALUES
(1,  'prod-0039', '������� AHD ����������� ����� AHD �-1�', 623, NULL,                 1, 5, 1),
(2,  'prod-0036','������� AHD ����������� Tecsar AHDW-1Mp-20Fl-eco ', 623, NULL,       1, 9, 1),--
(3,  'prod-0034','������� AHD ����������� Tecsar AHDW-1Mp-40Vfl ', 1471, NULL,         1, 7, 1),
(4,  'prod-0032','������� IP ������ Hikvision DS-2CD2042WD-I', 4180, NULL,             1, 15, 1),
(5,  'prod-0021','����������� AHD ��������� Tecsar AHDB-2Mp-0', 2117, NULL,            3, 23, 1),--

(6,  'prod-0017','���������������� AHD PoliceCam DVR-6604T', 1046, NULL,               2, 8, 1),
(7,  'prod-0014','���������������� AHD Tecsar HDVR Neo-Futurist', 1619, NULL,          2, 32, 1),
(8,  'prod-0012','���������������� Tecsar Neo-Futurist half-FHD', 1743, NULL,          2, 45, 1),

(9,  'prod-0011','������ �������� ITV ��-101', 202, NULL,                             1, 60, 1),
(10, 'prod-0010','������ �������� ��-101 � ������ PI (Pet Immune)', 209, NULL,        1, 14, 1),
(11, 'prod-0009','������������ ������ �������� ����� �-302', 525, NULL,               1, 16, 1),
(12, 'prod-0007','������������ ������ �������� Ajax MotionProtect ������', 859, NULL, 2, 17, 1),
(12, 'prod-0005','������������ ������ �������� Ajax MotionProtect �����', 871, NULL,  1, 5, 1)
GO


  --12**********************************
CREATE TABLE MyStore.dbo.Images (
  ImageId int NOT NULL IDENTITY CONSTRAINT PK_Images_ImageId PRIMARY KEY,
  ProductId int NOT NULL,
  ColorId INT NOT NULL, CONSTRAINT FK_Images_ColoredProducts FOREIGN KEY (ProductId, ColorId)  REFERENCES ColoredProducts (ProductId, ColorId),
  ImageBody VARBINARY(MAX) NULL, 
  ChangedName NVARCHAR(200) NULL,
  ImageNumber INT NULL
)
GO

--INSERT MyStore.dbo.Images(ProductId, ColorId, ImageBody, ChangedName) VALUES
--(1, 1, 0xFFD8FFE000104A46494600010101006000600000FFDB004300080606070605080707070909080A0C140D0C0B0B0C1912130F141D1A1F1E1D1A1C1C20242E2720222C231C1C2837292C30313434341F27393D38323C2E333432FFDB0043010909090C0B0C180D0D1832211C213232323232323232323232323232323232323232, 'aHYuanBn')

--GO


--13**********************************
 CREATE TABLE MyStore.dbo.DeliveryTypes (
  DeliveryTypeId int NOT NULL IDENTITY CONSTRAINT PK_DeliveryTypes_DeliveryTypeId PRIMARY KEY,
  DeliveryTypeName NVARCHAR(50) NOT NULL
)
GO

INSERT MyStore.dbo.DeliveryTypes(DeliveryTypeName) VALUES
('-'),
('����� �����'),
('��� �����'),
('������ ��������')
GO



  --DROP TABLE MyStore.dbo.Orders
--14**********************************
CREATE TABLE MyStore.dbo.Orders (
  OrderId int NOT NULL IDENTITY CONSTRAINT PK_Orders_OrderId PRIMARY KEY,
  OrderDate datetime NOT NULL,
  ClientId int NOT NULL  CONSTRAINT FK_Orders_Clients FOREIGN KEY (ClientId)  REFERENCES Clients (ClientId),
  
  Comment NVARCHAR(50) NULL,
  AdminComment nvarchar(50) NULL,
  EmployeeId INT NOT NULL CONSTRAINT FK_Orders_Employees FOREIGN KEY (EmployeeId)  REFERENCES Employees (EmployeeId),
  DeliveryTypeId int NOT NULL CONSTRAINT FK_Orders_DeliveryTypes FOREIGN KEY (DeliveryTypeId)  REFERENCES DeliveryTypes (DeliveryTypeId),
  Summ decimal NOT NULL,
  Completed BIT NOT NULL
)
GO

INSERT MyStore.dbo.Orders( OrderDate, ClientId, Comment, AdminComment, EmployeeId, DeliveryTypeId, Summ, Completed) VALUES
('20211110', 1,   NULL, NULL, 1, 1, 2366, 1),
('20210210', 2,   NULL, NULL, 2, 2, 1619, 1),
('20210311', 3,   NULL, NULL, 1, 3, 1046, 1),
('20210408', 5,   NULL, NULL, 1, 1, 734, 1),
('20210613', 4,   NULL, NULL, 2, 2, 623, 1),
('20210715', 4,   NULL, NULL, 1, 1, 859, 1),
('20210715', 2,   NULL, NULL, 2, 2, 1471, 1),
('20211016', 1,   NULL, NULL, 1, 3, 1743, 1),
('20210421', 3,   NULL, NULL, 2, 3, 209, 1),
( '20211110', 5,  '�������� ��� ���������', '�������� ��� ���������', 1, 2, 859, 1),
( '20220102', 3,  NULL, NULL, 2, 2, 202, 1),
( '20210723', 2,  NULL, NULL, 1, 2, 623, 1),
( '20210108', 1,  NULL, NULL, 2, 1, 623, 1),
( '20211210', 4,  NULL, NULL, 1, 2, 4180, 1)
GO



--15**********************************
CREATE TABLE MyStore.dbo.ProductValues (
  ProductValuesId int NOT NULL IDENTITY CONSTRAINT PK_ProductValues_ProductValuesId PRIMARY KEY,
  CharacteristicValuesId int NOT NULL CONSTRAINT FK_ProductValues_CharacteristicValues FOREIGN KEY(CharacteristicValuesId)  REFERENCES CharacteristicValues (CharacteristicValuesId),
  ProductId int NOT NULL CONSTRAINT FK_ProductValues_Products FOREIGN KEY (ProductId)  REFERENCES Products (ProductId),
 )
GO

INSERT MyStore.dbo.ProductValues(CharacteristicValuesId, ProductId) VALUES
(1, 5),
(2, 4),
(3, 1),
(3, 2),
(3, 3),
(4, 4),
(4, 3),
(4, 5),
(5, 1),
(5, 2),
(6, 4),
(6, 3),
(6, 5),
(7, 1),
(7, 2),
(8, 1),
(8, 2),
(8, 3),
(8, 4),
(8, 5),
(9, 1),
(9, 2),
(9, 4),
(10, 4),
(10, 5),
(11, 1),
(11, 2),
(11, 4),
(12, 2),
(12, 3),
(12, 4),
(12, 5),
(13, 1),
(13, 9),
(13, 10),
(13, 11),
(13, 12),
(14, 1),
(14, 2),
(14, 3),
(14, 4),
(14, 5),
(14, 6),
(14, 7),
(14, 8),
(15, 9),
(16, 12),
(17, 12),
(18, 1),
(18, 2),
(18, 3),
(18, 4),
(18, 5),
(18, 9),
(18, 10),
(18, 11),
(19, 4),
(20, 6),
(20, 7),
(20, 8),
(21, 6),
(21, 7),
(21, 8),
(22, 8),
(23, 1),
(23, 2),
(23, 3),
(23, 4),
(23, 5),
(23, 6),
(23, 7),
(23, 8),
(24, 6),
(24, 7),
(24, 8),
(25, 12),
(26, 9),
(26, 10),
(26, 11),
(26, 1),
(26, 2),
(26, 3),
(26, 4),
(26, 5),
(27, 10),
(27, 11),
(27, 12),
(28, 12),
(29, 10),
(29, 11),
(29, 12),
(30, 9),
(30, 10),
(30, 11),
(31, 11),
(31, 12),
(32, 9),
(32, 10),
(32, 11),
(33, 1),
(33, 5),
(33, 10),
(33, 11),
(34, 2),
(34, 3),
(34, 4),
(35, 12),
(36, 12)
GO

--DROP TABLE MyStore.dbo.Purchases
--16**********************************
CREATE TABLE MyStore.dbo.Purchases (
  OrderId int NOT NULL, CONSTRAINT FK_Purchases_Orders FOREIGN KEY (OrderId)  REFERENCES Orders (OrderId),
  ProductId int NOT NULL,
  ColorId int NOT NULL, CONSTRAINT FK_Purchases_ColoredProducts FOREIGN KEY (ProductId, ColorId)  REFERENCES ColoredProducts (ProductId, ColorId),
  Quantity int NOT NULL

  CONSTRAINT PK_Purchases PRIMARY KEY (ProductId, ColorId, OrderId)
)
GO

INSERT MyStore.dbo.Purchases(OrderId, ProductId, ColorId, Quantity) VALUES
(1,   1, 1,  1),
(1,   8, 2,  3),
(2,   7, 2,  5),
(3,   6, 2,  5),
(4,   11, 1, 2),
(4,   10, 1, 4),
(5,   5, 3, 31),
(5,   2, 1, 1),
(6,   12, 2, 2),
(7,   3, 1, 5),
(8,   8, 2, 3),
(9,   10, 1, 4),
(10,  12, 2, 1),
(11,  9, 1, 24),
(12,  2, 1, 1),
(13,  1, 1, 9),
(14,  4, 1, 1)
GO