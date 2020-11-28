/* Begin TT#155 - JScott - Add Size Curve info to Node Properties */
alter table "NODE_SIZE_CURVE_CRITERIA" 
	add constraint "NODE_SIZE_CURVE_CRITERIA_FK1" foreign key ("HN_RID")
	 references "HIERARCHY_NODE" ("HN_RID")  

go
