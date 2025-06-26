create Database Estudiantes

use Estudiantes

DROP TABLE Notas, Materias, Estudiantes;

--SELECT TABLAS

select * from Materias

select * from Notas

select * from Estudiantes

drop table Notas;

--CREAR TABLAS

CREATE TABLE Estudiantes (
Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
Nombre_y_Apellido VARCHAR(100) NOT NULL,
legajo VARCHAR(50) NOT NULL,
fecha_carga_estudiante DATETIME NOT NULL
);

CREATE TABLE Materias (

Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
CodigoMateria INT NOT NULL,
Nombre VARCHAR(45) NOT NULL
);

CREATE TABLE Notas (
Id INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
Id_alumno int,
Id_materia int,
nota DECIMAL,
Aprobado BIT NOT NULL DEFAULT 0
);


--INSERTAR VALORES
SET IDENTITY_INSERT Estudiantes off;
Insert into Estudiantes (Id, Nombre_y_Apellido, legajo, fecha_carga_estudiante)
values ('1','Joaquin RL','1234566','2025-06-10 20:30:00')

SET IDENTITY_INSERT Materias off;

Insert into Materias (Id, CodigoMateria, Nombre)
values ('8','8','Ingles')

----Matematica, Lengua, Historia, Geografia, Biología, Quimica, Fisica, Ingles

SET IDENTITY_INSERT Notas on;
Insert into Notas (Id, nota)
values ('1','9')

select * from Estudiantes 
select * from Materias 
select * from Notas 
--FORANIAS

ALTER TABLE Notas
ADD CONSTRAINT FK_Notas_Estudiantes
FOREIGN KEY (id) REFERENCES Estudiantes(id);

ALTER TABLE Notas
ADD CONSTRAINT FK_Notas_Materias
FOREIGN KEY (id) REFERENCES Materias(id);

--SELECT
SELECT Estudiantes.legajo, Estudiantes.Nombre_y_Apellido, 
Notas.id, Notas.nota
FROM Notas, Estudiantes
WHERE Estudiantes.id = Notas.id

--CAMBIAR NOMBRE COLUMNA
EXEC sp_rename 'Estudiantes.nombreyapellido', 'Nombre_y_Apellido', 'COLUMN';

--UPDATE
UPDATE Notas
SET Id_materia = 1
WHERE id = 1;


UPDATE Estudiantes
SET nombreyapellido = 'Joaquin Rivero Alvarez'
WHERE id = 1;