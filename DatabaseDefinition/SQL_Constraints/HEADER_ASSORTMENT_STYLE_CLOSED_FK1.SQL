alter table "ASSORTMENT_STYLE_CLOSED"
	add constraint "HEADER_ASSORTMENT_STYLE_CLOSED_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
