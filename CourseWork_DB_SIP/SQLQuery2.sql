CREATE DATABASE SIP; -- "����� ������� �����������" SIP Sphere_Insurance_Protection
GO

-- ��������� ������� "�볺���"
CREATE TABLE Clients (
    IdClients INT PRIMARY KEY,
    ClientFiName VARCHAR(50),
    ClientSeName VARCHAR(50),
    ClientFaName VARCHAR(50),
    ClientPhone VARCHAR(20),
    ClientAddress VARCHAR(100)
);

-- ��������� ������� "������"
CREATE TABLE Agents (
    IdAgent INT PRIMARY KEY,
    AgentFiName VARCHAR(50),
    AgentSeName VARCHAR(50),
    AgentFaName VARCHAR(50),
    AgentPhone VARCHAR(20),
    AgentPost VARCHAR(50)
);

-- ��������� ������� "�����"
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


-- ��������� ������� "����������� ���������"
CREATE TABLE InsuranceAuto (
    IdPolicies INT PRIMARY KEY,
    AutoNumber VARCHAR(20),
    AutoYear INT,
    AutoCost DECIMAL(10, 2),
    FOREIGN KEY (IdPolicies) REFERENCES Policies(IdPolicies)
);


-- ��������� ������� "����������� �����"
CREATE TABLE InsuranceHome (
    IdPolicies INT PRIMARY KEY,
    HomeAddress VARCHAR(100),
    HomeCost DECIMAL(10, 2),
    FOREIGN KEY (IdPolicies) REFERENCES Policies(IdPolicies)
);

-- ��������� ������� "����������� �����"
CREATE TABLE InsuranceLife (
    IdPolicies INT PRIMARY KEY,
    LifeFiName VARCHAR(50),
    LifeSeName VARCHAR(50),
    LifeFaName VARCHAR(50),
    LifeBirthDay DATE,
    FOREIGN KEY (IdPolicies) REFERENCES Policies(IdPolicies)
);

-- ��������� ������� "��������� �������"
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

-- ��������� ��� ��� ���������
CREATE ROLE Buchalteriya;
-- ������� ������� �� ������� Policies
GRANT SELECT ON Policies TO Buchalteriya;
-- ������� ������� �� ������� InsuranceEvent
GRANT SELECT ON InsuranceEvent TO Buchalteriya;

-- ��������� ������������ �� ������� �� ��� "����������"
CREATE LOGIN Bugalter1 WITH PASSWORD = '1234';
CREATE USER Bugalter1 FOR LOGIN Bugalter1;
ALTER ROLE Buchalteriya ADD MEMBER Bugalter1;

CREATE LOGIN Bugalter2 WITH PASSWORD = '4321';
CREATE USER Bugalter2 FOR LOGIN Bugalter2;
ALTER ROLE Buchalteriya ADD MEMBER Bugalter2;



-- ��������� ��� ��� ������
CREATE ROLE AgentRole;

-- ������� ������� �� ��� ������� (Clients, Agents, Policies, InsuranceAuto, InsuranceHome, InsuranceLife, InsuranceEvent)
GRANT SELECT ON Clients TO AgentRole;
GRANT SELECT ON Agents TO AgentRole;
GRANT SELECT ON Policies TO AgentRole;
GRANT SELECT ON InsuranceAuto TO AgentRole;
GRANT SELECT ON InsuranceHome TO AgentRole;
GRANT SELECT ON InsuranceLife TO AgentRole;
GRANT SELECT ON InsuranceEvent TO AgentRole;

-- ��������� ������������ �� ������� �� ��� "�����"
CREATE LOGIN AgentR1 WITH PASSWORD = '1234';
CREATE USER AgentR1 FOR LOGIN AgentR1;
ALTER ROLE AgentRole ADD MEMBER AgentR1;

CREATE LOGIN AgentR2 WITH PASSWORD = '4321';
CREATE USER AgentR2 FOR LOGIN AgentR2;
ALTER ROLE AgentRole ADD MEMBER AgentR2;
