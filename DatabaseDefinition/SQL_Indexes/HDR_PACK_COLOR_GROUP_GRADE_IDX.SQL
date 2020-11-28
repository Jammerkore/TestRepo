/*Begin RELEASE 4.2.0.5 07-09-2012 - RBeck - TT#1337 - Schemas Different Across Brands */
create unique index "HDR_PACK_COLOR_GROUP_GRADE_IDX" on "ASSORTMENT_MATRIX_DETAIL" (
	"HDR_RID",
	"HDR_PACK_RID",
	"COLOR_CODE_RID",
	"SGL_RID",
	"GRADE")  
GO
