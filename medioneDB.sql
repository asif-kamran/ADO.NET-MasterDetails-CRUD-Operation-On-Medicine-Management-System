
create database medioneDB
go
use medioneDB
go
create table companyInfo
(
	companyId int primary key,
	companyName varchar(50) not null unique
)
go
select * from companyInfo;
insert into companyInfo(companyId,companyName) values(1,'Beximco Pharmaceutical');
create table medicineForm
(
	formId int primary key,
	formName varchar(50) not null unique
)
go
select * from medicineForm;
insert into medicineForm(formId,formName) values(3,'Drop');
create table medicine
(
	medicineId int primary key,
	medicineName varchar(50) not null unique,
	companyId int references companyInfo(companyId),
	formId int references medicineForm(formId),
	medicineImage varbinary(max) null,
	MRP money not null
)
go

update medicine set medicineName='Napa 500',companyId=1 ,
formId=1 ,medicineImage='',MRP=500
where medicineId=1;

select * from medicine;
insert into medicine(medicineId,medicineName,companyId,formId,medicineImage,MRP) 
values (1,'napa',1,1,'',500)
go
select medicineId,medicineName,companyId,formId,medicineImage,MRP 
from medicine where medicineId=1
go
drop table purchase
go
create table purchase
(
	purchaseId int primary key,
	medicineId int references medicine(medicineId),
	quantity int not null,
	unitPrice money not null,
	purchaseDate date not null
)
go
select * from purchase
go

insert into purchase(purchaseId,medicineId,quantity,unitPrice,purchaseDate)
values (1,1,5,500,'01-01-2024')

select companyId,companyName from companyInfo where companyId=1 


update companyInfo set companyName='' where companyId=1

update medicineForm set formName='' where formId=1

select formId,formName from medicineForm where formId=1;

delete from medicineForm where formId=3

delete from medicine where medicineId=1 

select * from medicine

select medicineId,medicineName,companyId,formId,medicineImage,mrp from medicine
where medicineId=1;

select * from medicine

select medicineId,medicineName,companyName,formName,medicineImage,MRP from medicine m join companyInfo c on m.companyId=c.companyId
join medicineForm f on m.formId=f.formId
select * from purchase
select purchaseId,medicineId,quantity,unitPrice,purchaseDate from purchase
where purchaseId=1

select * from medicine join Stock on medicine.medicineId=Stock.medicineId;
drop table medicineSupplier;
create table medicineSupplier
(
	id int primary key identity(1,1),
	medicineId int references medicine(medicineId),
	supplierName varchar(50) not null,
	supplierAddress varchar(50) not null,
	disAmount int not null
)
insert into medicineSupplier(medicineId,supplierName,supplierAddress,disAmount)
values(1,'a','a',5)
select * from medicineSupplier
select supplierName,supplierAddress,disAmount from medicineSupplier where medicineId=8;

update medicineSupplier set supplierName='Kamran1', supplierAddress='130' ,disAmount=5
where id=1;