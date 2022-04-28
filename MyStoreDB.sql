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
INSERT MyStore.dbo.Employees(Name, SecondName, PassHash, Email) VALUES ('Василий', 'Пупкин', 'a', N'a@a' )
INSERT MyStore.dbo.Employees(Name, SecondName, PassHash, Email) VALUES ('Иван', 'Иванов', 'b', N'b@b')
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
( '20211110', N'ligula.nullam@aol.org', 'p', '14434757694', 'Сourier ', 'Гаврилов', 'Макар', 'Запорожье', 'Строителей', N'10', N'86', 5464, 45),
( '20210913', N'dis@google.org', 'p', '18746182285', 'UkrPost', 'Орлова', 'Арина', 'Киев', 'Вербицкого', N'5', N'65', 3412, 21),
( '20201225', N'orci.ut@yahoo.ca', 'p', '12227435874', 'Npost', 'Соловьев', 'Марк', 'Луцк', 'Свободы', N'9', N'76', 1113, NULL),
( '20190512', N'nunc@protonmail.org', 'p', '17229072538', 'Сourier ', 'Позднякова', 'Валерия', 'Днепр', 'Хмельницкого', N'14', N'21', 3113, 44),
( '20190512', N'dolor.sit@hotmail.org', 'p', '12473165122', 'Сourier ', 'Голиков', 'Дмитрий', 'Одесса', 'Ришельевская', N'27', N'78', NULL, 13)
GO


--3**********************************
CREATE TABLE MyStore.dbo.Characteristics (
  CharacteristicId int NOT NULL IDENTITY CONSTRAINT PK_DetailsKeys_DetailKeyId PRIMARY KEY,
  CharacteristicName nvarchar(50) NULL UNIQUE --хоть характеристики и распределены по категориям, но у конкретного товара может и не быть характеристики, по этому NULL

)
GO

INSERT MyStore.dbo.Characteristics(CharacteristicName) VALUES
('Разрешающая способность'),--1
('Размер матрицы'),         --2
('Тип сенсора'),            --3
('Балланс белого'),         --4
('Количество ИК диодов подсветки'),--5
('Расстояние подсветки'),          --6
('Возможность установки на улице'),--7
('Напряжение питания'),          --8
('Угол обзора'),                 --9
('Тип передачи данных'),         --10
('Тип входного сигнала'),        --11
('Тип записи'),                  --12
('Беспроводная передача данных'),--13
('Расстояние обнаружения'),      --14
('Максимальное расстояние передачи сигнала'),--15
('Функция игнорирования животных'),          --16
('Высота установки'),            --17
('Защита от взлома'),            --18
('Потребляемый ток'),            --19
('Угол по горизонтали'),         --20
('Угол по вертикали'),           --21
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
('24 шт'),--9
('20 м'),--10
('Есть'),--11
('12 В'),--12
('IP ONVIF'),--13
('4 BNC'),--14
('SATA HDD'),--15
('Да '),--16
('Пироэлектрический'),--17
('14 м'),--18
('2000 м'),--19
('1,8… 2,4 м, рекомендуемая 2,2 м'),--20
('3В'),--21
('8мА/18мА (с вЫключенным/включенным светодиодом)'),--22
('90 град'),--23
('85 град'),--24
('88,5 град'),--25
('57 град'),--26
('Нет'),--27
('AHD'),--28
('5 В'),--29
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
( 1, 1, 1),--1     Разрешающая способность
( 1, 2, 2),--2     Размер матрицы
( 1, 3, 3),--3     Тип сенсора
( 1, 4, 4),--4     Балланс белого
( 1, 5, 5),--5     Количество ИК диодов подсветки
( 1, 6, 6),--6     Расстояние подсветки
( 1, 7, 7),--7     Возможность установки на улице
( 1, 8, 8),--8     Напряжение питания
( 1, 9, 9),--9     Угол обзора
( 1, 13, 10),--10  Беспроводная передача данных
( 1, 10, 11),--11  Тип передачи данных
( 2, 11, 1),--12   Тип входного сигнала
( 2, 8,  2), --13  Напряжение питания
( 2, 12, 3),--14   Тип записи
( 3, 13, 1),--15   Беспроводная передача данных
( 3, 14, 2),--16   Расстояние обнаружения
( 3, 15, 3),--17   Максимальное расстояние передачи сигнала
( 3, 16, 4),--18   Функция игнорирования животных
( 3, 17, 5),--19   Высота установки
( 3, 18, 6),--20   Защита от взлома
( 3, 19, 7),--21   Потребляемый ток
( 3, 20, 8),--22   Угол по горизонтали
( 3, 21, 9),--23   Угол по вертикали
( 3, 8,  10)--24   Напряжение питания
GO

--7**********************************
CREATE TABLE MyStore.dbo.CharacteristicValues (
  CharacteristicValuesId int NOT NULL IDENTITY CONSTRAINT PK_CharacteristicValues_CharacteristicValuesId PRIMARY KEY,
   
  CategoryCharacteristicsId int NOT NULL CONSTRAINT FK_CharacteristicValues_CategoryCharacteristics FOREIGN KEY (CategoryCharacteristicsId)  REFERENCES CategoryCharacteristics (CategoryCharacteristicsId),
  ValueId int NOT NULL CONSTRAINT FK_CharacteristicValues_Values FOREIGN KEY (ValueId)  REFERENCES [Values] (ValueId)
)
GO

INSERT MyStore.dbo.CharacteristicValues(CategoryCharacteristicsId, ValueId) VALUES 
(1, 1),--    Разрешающая способность         2 Mp (1980p)
(1, 2),--    Разрешающая способность         4 Mp 2688x1520
(1, 3),--    Разрешающая способность         1 Mp (720p)
(2, 4),--    Размер матрицы                  1/3"
(2, 5),--    Размер матрицы                  1/4"
(3, 6),--    Тип сенсора--                   CMOS
(3, 7),--    Тип сенсора                     OmniVision OV9712
(4, 8),--    Балланс белого                  Auto
(5, 9),--    Количество ИК диодов подсветки  24 шт
(6, 10),--   Расстояние подсветки            20 м
(6, 18),--   Расстояние подсветки            14 м 
(7, 16),--   Возможность установки на улице  Да
(7, 27),--   Возможность установки на улице  Нет
(8, 12),--   Напряжение питания              12 В
(9, 24),--   Угол обзора                     85 град
(9, 26),--   Угол обзора                     57 град
(10, 11),--  Беспроводная передача данных    Есть
(10, 27),--  Беспроводная передача данных    Нет
(11, 13),--  Тип передачи данных             IP ONVIF
(11, 28),--  Тип передачи данных             AHD
(12, 14),--  Тип входного сигнала            4 BNC
(12, 13),--  Тип входного сигнала            IP ONVIF
(13, 12),--  Напряжение питания              12 В
(14, 28),--  Тип записи                      AHD 
(15, 11),--  Беспроводная передача данных    Есть
(15, 27),--  Беспроводная передача данных    Нет
(16, 18),--  Расстояние обнаружения          14 м
(17, 19),--  Максимальное расстояние передачи сигнала  2000 м
(18, 16),--  Функция игнорирования животных  Да
(19, 20),--  Высота установки                1,8… 2,4 м, рекомендуемая 2,2 м
(20, 11),--  Защита от взлома                Есть
(21, 22),--  Потребляемый ток                8 мА/18 мА (с вЫключенным/включенным светодиодом)
(22, 23),--  Угол по горизонтали             90 град
(22, 25),--  Угол по горизонтали             88,5 град
(23, 23),--  Угол по вертикали               90 град
(24, 21), --  Напряжение питания              3В
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
  CategoryId int NOT NULL --CONSTRAINT FK_Products_CategoryCharacteristics FOREIGN KEY (CategoryId)  REFERENCES CategoryCharacteristics (CategoryId),--или на Categories 
                          CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId)  REFERENCES Categories (CategoryId),
  SectionId INT NOT NULL CONSTRAINT FK_Products_Sections FOREIGN KEY (SectionId)  REFERENCES Sections (SectionId),
  Description nvarchar(4000) NULL
)
GO

INSERT MyStore.dbo.Products(CategoryId, SectionId, Description) VALUES
(1, 1, 'Уличная видеокамера Страж AHD У-1М - проводная камера, работающая по технологии AHD предназначенная для видеонаблюдения в режиме реального времени на любых объектах. Данная камера работает с любыми устройствами записи и платами захвата, которые поддерживают стандарт высокого разрешения AHD 720p. Это могут быть и гибридные видеорегистраторы, поддерживающие одновременную работу с AHD и IP камерами. Работает система по следующему принципу: камера подключается к источнику питания и видеорегистратору при помощи проводов. Камера запечатляет окружающую среду, и по проводу передает видео на видеорегистратор или плату видеозахвата.'),
(1, 1, 'Уличная видеокамера Tecsar AHDW-1Mp-20Fl-eco (новая модификация вместо прежней Tecsar AHDW-1M-20F-eco с идентичными характеристиками) - проводная камера, работающая по технологии AHD предназначенная для видеонаблюдения в режиме реального времени на любых объектах. Данная камера работает с любыми устройствами записи и платами захвата, которые поддерживают стандарт высокого разрешения AHD 720p. Это могут быть и гибридные видеорегистраторы, поддерживающие одновременную работу с AHD и IP камерами. Работает система по следующему принципу: камера подключается к источнику питания и видеорегистратору при помощи проводов. Камера запечатляет окружающую среду, и по проводу передает видео на видеорегистратор или плату видеозахвата.'),
(1, 1, 'Tecsar AHDW-1Mp-40Vfl - многофункциональная защищенная уличная камера. Новый вариант популярной модели Tecsar AHDW-1M-40V с аналогичными характеристиками. Подходит для любых объектов. Максимальное количество мощных инфракрасных светодиодов обеспечивает яркое освещение охраняемого объекта. Изображение с матрицы обрабатывается производительным процессором и передается в стандарте AHD. Регулируемое фокусное расстояние позволяет устанавливать желаемый угол обзора от широкого, как у дверного "глазка" до узкого, позволяющего "приближать" предметы в несколько раз. Разрешающая способность камер AHD и, Tecsar AHDW-1Mp-40Vfl в том числе, намного превосходит аналогичные показатели для прежних обычных аналоговых устройств, но в то же время они не создают задержек и не требуют подключения коммутаторов, как IP камеры. Для работы необходим только видеорегистратор или плата захвата, с поддержкой режима AHD. Это могут быть и гибридные видеорегистраторы, поддерживающие работу с AHD, IP и обычными аналоговыми камерами. Работает система по следующему принципу: камера подключается к источнику питания и видеорегистратору при помощи проводов. Камера снимает окружающую среду, и передает видео на видеорегистратор или плату видеозахвата. '),
(1, 1, 'IP камера класса Люкс с максимальным разрешением 4Мп 2688x1520 точек при частоте кадров 20 к/с
Широкий формат видео при всех настройках. Решим максимального разрешения похож на извеестный формат 5Мп с обрезанными верхней и нижней частью изображения, и на 3Мп (2048 x 1536) пикселей расширенный по горизонтали что в большинстве применений позволяет использовать оборудование более эфективно.
Новая производительная функция WDR 120дБ обеспечивает действительно качественную одновременную различаемость деталей затемненной и переосвещенной частей кадра. Камера без WDR всегда "заливает" белым цветом участок под солнцем или черным цветом участок в тени. wdr значительно улучшает ситуацию (см верхний рисунок), а видеокамера hikvision DS-2CD2042WD-I с WDR 120dB  доводит изображение до идеального, выравнивая освещенность на сколько возможно.'),
(1, 1, 'Функции и особенности
Полная совместимость со всеми запсывающими устройствами Tecsar - сетевыми регистраторами NVR, гибридными видеорегистраторами HVR AHD видеонаблюдения, гибридными видеорегистраторами CVBS
Камера оснащена чувствительной матрицей 1/4" Omnivision 2 Megapixel CMOS Sensor
Процессор HiSilicon HI3518C, обеспечивает основной поток записи 720p при частоте кадров 25 к/с, поддерживает все функции для получения безупречного изображения в любых условиях съемки
Подсветка на инфракрасных 5-мм светодиодах позволяет видеть на расстояние 20 метров в полной темноте
Исполнение в металлическом корпусе со степенью защиты IP66
Поддержка технологии PoE (Power over Ethernet), позволяющей питать устройство и передавать видеосигнал по одному кабелю "витая пара"
Камера имеет удобно структрированный графический web интерфейс? защищенный паролем, через который осуществляется доступ ко всем функциям и настройкам.
Поддержка алгоритмов шифрования AES, DES, 3DES'),
(2, 1, '4-х канальный видеорегистратор с поддержкой 3-х типов камер: простые аналоговые (CVBS), AHD 1 и 1,3 Мп, IP. Дополнительные бонусы - 4 канала аудио, дополнительный скоростной видеовыход HDMI, интерфейс управления роботизированными камерами (PTZ). Поддержка онлайн видео на мобильных устройствах, режимы автоматической записи. '),
(2, 1, 'Современный гибридный видеорегистратор в эргономичном корпусе. Предназначен для работы в небольших системах видеонаблюдения, а также организации сетей, объединяющих до 256 видеорегистраторов. Представляет из себя специализированный компьютер, предназначенный для круглосуточной записи и воспроизведения видео с камер наблюдения. Устройство принимает видеосигналы с AHD или аналоговых камер через 4 BNC разъема, видеосигнал с IP камер - через сетевой разъем LAN. Для работы с видео используется обыкновенный монитор или телевизор с разъемом VGA. Мышка поставляется в комплекте. Видеорегистратор Tecsar HDVR Neo-Futurist позволяет просматривать видео в реальном времени, воспроизводить видеоархив, получать полный доступ для просмотра и настроек с любого компьютера или мобильного устройства по сети интернет. Предусмотрен бесплатный доступ к облачному сервису tecsar-cloud, который позволяет обойтись без внешнего IP адреса в месте установки регистратора. Доступна запись по движению с регулировкой чувствительности, в том числе можно настроить запись при движении только в заданных местах кадра.'),
(2, 1, 'Легкий видеорегистратор в эргономичном корпусе, предназначенный для качественных систем видеонаблюдения при минимальном бюджете на любых объектах. Принимает изображение с аналоговых и/или цифровых камер по проводам. Способен показывать,  обнаруживать движение и записывать изображение со звуком с аналоговых и IP камер. Выводит изображение на монитор с VGA разъемом или обычный телевизор. Полная поддержка сети: просмотр всех видеокамер, архива с указанием времени, пользовательские настройки. Присутствует весь базовый функционал для охранного видеонаблюдения.'),
(3, 3,'Самый простой качественный датчик движения: сдвоенный пироэлектрический сенсор для фильтрации от помех обеспечивает достаточную эфективность работы в большинстве применений.
Предназначение - обнаружение движений человека
Оснащен двойным пироэлектрическим сенсором
Функция самодиагностики
Автоматическая компенсация температурных перепадов
Приспособлен к работе при низких температурах до -10°
Расстояние до движущегося объекта 18 м при горизонтальном угле обзора 90°
Регулировка положения сенсора под линзой 
Оптоэлектронное реле
Корпус выполнен из прочного, качественного пластика'),
(3, 3,'Самый простой качественный датчик движения: сдвоенный пироэлектрический сенсор для фильтрации от помех и специальная линза обеспечивает достаточную эфективность работы в большинстве применений.
Предназначение - обнаружение движений человека
Защита от срабатывания на животных
Оснащен двойным пироэлектрическим сенсором
Функция самодиагностики
Автоматическая компенсация температурных перепадов
Приспособлен к работе при низких температурах до -10°
Расстояние до движущегося объекта 18 м при горизонтальном угле обзора 90°
Регулировка положения сенсора под линзой 
Оптоэлектронное реле
Корпус выполнен из прочного, качественного пластика'),
(3, 3,'Датчик движения без проводов: Недорогая модель для быстрой установки, которая позволяет не портить внутренний ремонт помещения и не срабатывает на домашних животных до 25 кг. Модель позволяет минимальными усилиями обезопасить помещение.
 
Максимальное расстояние до централи до 100 метров
Питание от двух пальчиковых батареек типа «AA» до 12 месяцев
Защищен от ложных срабатываний
Дальность определения движения человека 12 метров
Регулировка чувствительности
Игнорирование кошек и собак весом до 25 килограмм
Горизонтальный угол обзора - 110°
Передает тревожный сигнал на частоте общего назначения  433 МГц, которая не требует специальной лицензии
Работает со всеми центральными блоками «Страж»: ECONOM, COMFORT, MULTIZONE, MULTIZONE II, CYCLOP II'),
(3, 3, 'Совершенный датчик движения: лучшая надежность, стильный дизайн, совместим с Ajax ocBridge.
 
Максимальное расстояние до централи до 2000 метров
Регулярная связь с централью
Регулируемый период опроса датчиков
Кодированный помехозащищенный сигнал
Работа от батарейки до 7 лет
Специальный алгоритм распознавания человека
дальность определения 14 метров
регулировка чувствительности
Игнорирование домашних животных весом до 20-ти кг, высотой до 50 см
Горизонтальный угол обзора - 88,5°
Использует частоту общего назначения  868 МГц, которая не требует специальной лицензии
Оповещение о разряде батарейки, срабатывании тампера, попытке глушения радиосигнала')
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
(1,  'prod-0039', 'Уличная AHD видеокамера Страж AHD У-1М', 623, NULL,                 1, 5, 1),
(2,  'prod-0036','Уличная AHD видеокамера Tecsar AHDW-1Mp-20Fl-eco ', 623, NULL,       1, 9, 1),--
(3,  'prod-0034','Уличная AHD видеокамера Tecsar AHDW-1Mp-40Vfl ', 1471, NULL,         1, 7, 1),
(4,  'prod-0032','Уличная IP камера Hikvision DS-2CD2042WD-I', 4180, NULL,             1, 15, 1),
(5,  'prod-0021','Видеокамера AHD корпусная Tecsar AHDB-2Mp-0', 2117, NULL,            3, 23, 1),--

(6,  'prod-0017','Видеорегистратор AHD PoliceCam DVR-6604T', 1046, NULL,               2, 8, 1),
(7,  'prod-0014','Видеорегистратор AHD Tecsar HDVR Neo-Futurist', 1619, NULL,          2, 32, 1),
(8,  'prod-0012','Видеорегистратор Tecsar Neo-Futurist half-FHD', 1743, NULL,          2, 45, 1),

(9,  'prod-0011','Датчик движения ITV КС-101', 202, NULL,                             1, 60, 1),
(10, 'prod-0010','Датчик движения КС-101 с линзой PI (Pet Immune)', 209, NULL,        1, 14, 1),
(11, 'prod-0009','Беспроводной датчик движения Страж М-302', 525, NULL,               1, 16, 1),
(12, 'prod-0007','Беспроводной датчик движения Ajax MotionProtect черный', 859, NULL, 2, 17, 1),
(12, 'prod-0005','Беспроводной датчик движения Ajax MotionProtect белый', 871, NULL,  1, 5, 1)
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
('Новая почта'),
('Укр Почта'),
('Курьер магазина')
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
( '20211110', 5,  'Пришлите мне подарочек', 'Прислать ему подарочек', 1, 2, 859, 1),
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