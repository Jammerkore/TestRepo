alter table "SYSTEM_OPTIONS"
	add constraint "SP_SYSTEM_OPTIONS_FK1" foreign key (
		"COMPANY_SP_ABBREVIATION")
	 references "STATE_PROVINCE" (
		"SP_ABBREVIATION")  

go
