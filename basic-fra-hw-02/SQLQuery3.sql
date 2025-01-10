USE CinemaDb_nova;

CREATE TABLE [CINEMA] (
    [cinema_id] NVARCHAR(50) NOT NULL,
    [name] NVARCHAR(255) NOT NULL,
    [location] NVARCHAR(255) NOT NULL,
    PRIMARY KEY ([cinema_id])
);

CREATE TABLE [CINEMA_HALL] (
    [hall_id] NVARCHAR(50) NOT NULL,
    [cinema_id] NVARCHAR(50) NOT NULL,
    [name] NVARCHAR(255) NOT NULL,
    [capacity] INT NOT NULL,
    PRIMARY KEY ([hall_id]),
    FOREIGN KEY ([cinema_id]) REFERENCES [CINEMA]([cinema_id])
);

CREATE TABLE [MOVIE] (
    [movie_id] NVARCHAR(50) NOT NULL,
    [cinema_id] NVARCHAR(50) NOT NULL,
    [hall_id] NVARCHAR(50) NOT NULL,
    [title] NVARCHAR(255) NOT NULL,
    [description] NVARCHAR(MAX) NOT NULL,
    [duration] INT NOT NULL,
    PRIMARY KEY ([movie_id]),
    FOREIGN KEY ([cinema_id]) REFERENCES [CINEMA]([cinema_id]) ON DELETE CASCADE,
    FOREIGN KEY ([hall_id]) REFERENCES [CINEMA_HALL]([hall_id]) ON DELETE CASCADE
);

CREATE TABLE [PERSON] (
    [person_id] NVARCHAR(50) NOT NULL,
    [name] NVARCHAR(255) NOT NULL,
    [password] NVARCHAR(255) NOT NULL,
    [role] NVARCHAR(50) NOT NULL CHECK ([role] IN ('admin', 'user')),
    PRIMARY KEY ([person_id])
);

CREATE TABLE [TICKET] (
    [ticket_id] NVARCHAR(50) NOT NULL,
    [person_id] NVARCHAR(50) NOT NULL,
    [movie_id] NVARCHAR(50) NOT NULL,
    [hall_id] NVARCHAR(50) NOT NULL,
    [cinema_id] NVARCHAR(50) NOT NULL,
    [show_time] DATETIME NOT NULL,
    [seat_number] NVARCHAR(50) NOT NULL,
    PRIMARY KEY ([ticket_id]),
    FOREIGN KEY ([hall_id]) REFERENCES [CINEMA_HALL]([hall_id]),
    FOREIGN KEY ([movie_id]) REFERENCES [MOVIE]([movie_id]),
    FOREIGN KEY ([person_id]) REFERENCES [PERSON]([person_id])
);
