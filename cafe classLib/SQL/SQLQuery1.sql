USE [vilken_enterprises_dk_db_DBMS]

Drop Table if exists Månedsplan
Drop Table if exists Vagt
Drop Table if exists Hold
Drop table if exists Ansatte
Drop table if exists Belastning

Create Table Ansatte
(
AnsatteID INT PRIMARY KEY,
Navn NVARCHAR(100),
Telefonnummer NVARCHAR(15),
Email NVARCHAR(100),
ErLeder BIT
)

Create Table Hold
(
HoldID INT PRIMARY KEY,
Navn NVARCHAR(100)
)

Create Table Vagt
(
VagtID INT PRIMARY KEY,
StartTid DATETIME,
SlutTid DATETIME,
AntalTimer AS DATEDIFF(HOUR, StartTid, SlutTid),
Hold int,
Foreign Key (Hold) References Hold(HoldID)
)



Create Table Månedsplan
(
MånedsplanID INT PRIMARY KEY,
AnsatteID INT,
Foreign Key (AnsatteID) References Ansatte(AnsatteID),
VagtID INT,
Foreign Key (VagtID) References Vagt(VagtID)
)

-- Ansatte

INSERT INTO Ansatte (AnsatteID, Navn, Telefonnummer, Email, ErLeder)
VALUES
(1, 'Pedro Garcia', '20112233', 'pedro@cantina.dk', 1),
(2, 'Jacoby Hansen', '20223344', 'jacoby@cantina.dk', 1),
(3, 'Emma Nielsen', '20334455', 'emma@cantina.dk', 0),
(4, 'Mads Jensen', '20445566', 'mads@cantina.dk', 0),
(5, 'Sofie Larsen', '20556677', 'sofie@cantina.dk', 0),
(6, 'Oliver Madsen', '20667788', 'oliver@cantina.dk', 0),
(7, 'Freja Andersen', '20778899', 'freja@cantina.dk', 0),
(8, 'Noah Pedersen', '20889900', 'noah@cantina.dk', 0);

-- Hold

INSERT INTO Hold (HoldID, Navn)
VALUES
(1, 'Morgen'),
(2, 'Eftermiddag');

-- Vagter

INSERT INTO Vagt (VagtID, StartTid, SlutTid, Hold)
VALUES
(1, '2026-10-01 09:00:00', '2026-10-01 14:00:00', 1),
(2, '2026-10-01 14:00:00', '2026-10-01 19:00:00', 2),

(3, '2026-10-02 09:00:00', '2026-10-02 14:00:00', 1),
(4, '2026-10-02 14:00:00', '2026-10-02 19:00:00', 2),

(5, '2026-10-03 09:00:00', '2026-10-03 14:00:00', 1),
(6, '2026-10-03 14:00:00', '2026-10-03 19:00:00', 2),

(7, '2026-10-04 09:00:00', '2026-10-04 14:00:00', 1),
(8, '2026-10-04 14:00:00', '2026-10-04 19:00:00', 2),

(9, '2026-10-05 09:00:00', '2026-10-05 14:00:00', 1),
(10, '2026-10-05 14:00:00', '2026-10-05 19:00:00', 2),

(11, '2026-10-06 09:00:00', '2026-10-06 14:00:00', 1),
(12, '2026-10-06 14:00:00', '2026-10-06 19:00:00', 2),

(13, '2026-10-07 09:00:00', '2026-10-07 14:00:00', 1),
(14, '2026-10-07 14:00:00', '2026-10-07 19:00:00', 2);

-- Månedsplan

INSERT INTO Månedsplan (MånedsplanID, AnsatteID, VagtID)
VALUES
(1, 1, 1),
(2, 3, 1),
(3, 4, 1),

(4, 2, 2),
(5, 5, 2),
(6, 6, 2),

(7, 1, 3),
(8, 7, 3),
(9, 8, 3),

(10, 2, 4),
(11, 3, 4),
(12, 5, 4),

(13, 1, 5),
(14, 4, 5),
(15, 6, 5),

(16, 2, 6),
(17, 7, 6),
(18, 8, 6),

(19, 1, 7),
(20, 3, 7),
(21, 5, 7),

(22, 2, 8),
(23, 4, 8),
(24, 6, 8),

(25, 1, 9),
(26, 7, 9),
(27, 8, 9),

(28, 2, 10),
(29, 3, 10),
(30, 5, 10),

(31, 1, 11),
(32, 4, 11),
(33, 6, 11),

(34, 2, 12),
(35, 7, 12),
(36, 8, 12),

(37, 1, 13),
(38, 3, 13),
(39, 5, 13),

(40, 2, 14),
(41, 4, 14),
(42, 6, 14);

-- Vis månedsplan

SELECT
    V.StartTid,
    V.SlutTid,
    H.Navn AS Hold,
    A.Navn AS Medarbejder
FROM Månedsplan M
INNER JOIN Ansatte A ON M.AnsatteID = A.AnsatteID
INNER JOIN Vagt V ON M.VagtID = V.VagtID
INNER JOIN Hold H ON V.Hold = H.HoldID
ORDER BY V.StartTid;

-- Belastning

SELECT
    A.Navn,
    COUNT(*) AS AntalVagter,
    SUM(V.AntalTimer) AS AntalTimer
FROM Månedsplan M
INNER JOIN Ansatte A ON M.AnsatteID = A.AnsatteID
INNER JOIN Vagt V ON M.VagtID = V.VagtID
GROUP BY A.Navn
ORDER BY AntalTimer DESC;

-- Kontaktoplysninger

SELECT
    Navn,
    Telefonnummer,
    Email
FROM Ansatte;

-- Belastning hen over året

SELECT
    A.Navn,
    YEAR(V.StartTid) AS Aar,
    MONTH(V.StartTid) AS Måned,
    COUNT(*) AS AntalVagter,
    SUM(V.AntalTimer) AS AntalTimer
FROM Månedsplan M
INNER JOIN Ansatte A ON M.AnsatteID = A.AnsatteID
INNER JOIN Vagt V ON M.VagtID = V.VagtID
GROUP BY
    A.Navn,
    YEAR(V.StartTid),
    MONTH(V.StartTid)
ORDER BY
    A.Navn,
    Aar,
    Måned;


    SELECT name
FROM sys.databases;

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES;

SELECT DB_NAME() AS AktuelDatabase;


SELECT @@SERVERNAME;
