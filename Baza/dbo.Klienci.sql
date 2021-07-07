CREATE TABLE [dbo].[Table]
(
	[Id klienta] INT NOT NULL PRIMARY KEY, 
    [Imię] NCHAR(10) NOT NULL, 
    [Nazwisko] NCHAR(10) NOT NULL, 
    [Password] NCHAR(10) NOT NULL, 
    [Numer rachunku] INT NOT NULL, 
    [Data założenia] DATE NOT NULL, 
    [Rodzaj użytkownika] NCHAR(10) NOT NULL, 
    [Telefon] INT NOT NULL, 
    [Miasto] NCHAR(10) NOT NULL, 
    [Ulica] NCHAR(10) NOT NULL, 
	[Srodki] INT NOT NULL,
    CONSTRAINT [PK_Table] PRIMARY KEY ([Id])
)
