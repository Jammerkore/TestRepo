/* Add the remaining keys, constraints and indexes for the table "SCHEDULE".                  */
alter table "SCHEDULE" add constraint "SCHEDULE_UC1" unique (
	"SCHED_NAME")  


go
