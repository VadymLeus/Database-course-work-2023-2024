CREATE DATABASE SIP; -- "Сфера захисту страхування" SIP Sphere_Insurance_Protection
GO

-- Створення таблиці "Клієнти"
CREATE TABLE Clients (
    IdClients INT PRIMARY KEY,
    ClientFiName VARCHAR(50),
    ClientSeName VARCHAR(50),
    ClientFaName VARCHAR(50),
    ClientPhone VARCHAR(20),
    ClientAddress VARCHAR(100)
);

-- Створення таблиці "Агенти"
CREATE TABLE Agents (
    IdAgent INT PRIMARY KEY,
    AgentFiName VARCHAR(50),
    AgentSeName VARCHAR(50),
    AgentFaName VARCHAR(50),
    AgentPhone VARCHAR(20),
    AgentPost VARCHAR(50)
);

-- Створення таблиці "Поліси"
CREATE TABLE Policies (
    IdPolicies INT PRIMARY KEY,
    StartDate DATE,
    EndDate DATE,
    SummaPol DECIMAL(10, 2),
	PoliciesType VARCHAR(50),
    IdClients INT,
    IdAgent INT,
	PolicyStatus VARCHAR(20),
    FOREIGN KEY (IdClients) REFERENCES Clients(IdClients),
    FOREIGN KEY (IdAgent) REFERENCES Agents(IdAgent)
);


-- Створення таблиці "Страхування автомобіля"
CREATE TABLE InsuranceAuto (
    IdPolicies INT PRIMARY KEY,
    AutoNumber VARCHAR(20),
    AutoYear INT,
    AutoCost DECIMAL(10, 2),
    FOREIGN KEY (IdPolicies) REFERENCES Policies(IdPolicies)
);


-- Створення таблиці "Страхування житла"
CREATE TABLE InsuranceHome (
    IdPolicies INT PRIMARY KEY,
    HomeAddress VARCHAR(100),
    HomeCost DECIMAL(10, 2),
    FOREIGN KEY (IdPolicies) REFERENCES Policies(IdPolicies)
);

-- Створення таблиці "Страхування життя"
CREATE TABLE InsuranceLife (
    IdPolicies INT PRIMARY KEY,
    LifeFiName VARCHAR(50),
    LifeSeName VARCHAR(50),
    LifeFaName VARCHAR(50),
    LifeBirthDay DATE,
    FOREIGN KEY (IdPolicies) REFERENCES Policies(IdPolicies)
);

-- Створення таблиці "Страховий випадок"
CREATE TABLE InsuranceEvent (
    IdPolicies INT,
    IdEvent INT,
    EventDate DATE,
    EventSumma DECIMAL(10, 2),
    EventReason VARCHAR(100),
    PRIMARY KEY (IdPolicies, IdEvent),
    FOREIGN KEY (IdPolicies) REFERENCES Policies(IdPolicies)
);

SELECT * FROM Agents;
SELECT * FROM Clients;
SELECT * FROM InsuranceLife;
SELECT * FROM Policies;
SELECT * FROM InsuranceHome;

DELETE FROM Policies
WHERE IdPolicies = 5;


USE SIP;

-- Створення ролі для бухгалтерії
CREATE ROLE Buchalteriya;
-- Надання доступу до таблиці Policies
GRANT SELECT ON Policies TO Buchalteriya;
-- Надання доступу до таблиці InsuranceEvent
GRANT SELECT ON InsuranceEvent TO Buchalteriya;

-- Створення користувачів та надання їм ролі "Бухгалтерія"
CREATE LOGIN Bugalter1 WITH PASSWORD = '1234';
CREATE USER Bugalter1 FOR LOGIN Bugalter1;
ALTER ROLE Buchalteriya ADD MEMBER Bugalter1;

CREATE LOGIN Bugalter2 WITH PASSWORD = '4321';
CREATE USER Bugalter2 FOR LOGIN Bugalter2;
ALTER ROLE Buchalteriya ADD MEMBER Bugalter2;



-- Створення ролі для агентів
CREATE ROLE AgentRole;

-- Надання доступу до всіх таблиць (Clients, Agents, Policies, InsuranceAuto, InsuranceHome, InsuranceLife, InsuranceEvent)
GRANT SELECT ON Clients TO AgentRole;
GRANT SELECT ON Agents TO AgentRole;
GRANT SELECT ON Policies TO AgentRole;
GRANT SELECT ON InsuranceAuto TO AgentRole;
GRANT SELECT ON InsuranceHome TO AgentRole;
GRANT SELECT ON InsuranceLife TO AgentRole;
GRANT SELECT ON InsuranceEvent TO AgentRole;

-- Створення користувачів та надання їм ролі "Агент"
CREATE LOGIN AgentR1 WITH PASSWORD = '1234';
CREATE USER AgentR1 FOR LOGIN AgentR1;
ALTER ROLE AgentRole ADD MEMBER AgentR1;

CREATE LOGIN AgentR2 WITH PASSWORD = '4321';
CREATE USER AgentR2 FOR LOGIN AgentR2;
ALTER ROLE AgentRole ADD MEMBER AgentR2;
