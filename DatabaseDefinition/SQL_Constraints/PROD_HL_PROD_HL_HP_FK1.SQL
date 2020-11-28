/* BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type */
ALTER TABLE "PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE"
	add constraint "PROD_HL_PROD_HL_HP_FK1" foreign key (
		"PH_RID",
		"PHL_SEQUENCE")
	references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")
go
