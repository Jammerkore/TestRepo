/*RELEASE 5.3.0.11 09-05-2013 - TT#488 - MD - Jellis - Group Allocation pt2*/
exec SP_MID_SET_TEXT 085119,'Invalid access of "ClearAssortment()" method by header [{0}]. Only an AssortmentProfile may clear the assortment.', 250,0,7,null,0
exec SP_MID_SET_TEXT 085120,'Assortment/Group Allocation [{0}] cannot be cleared when units [{1}] are allocated to non-reserve stores.', 250,0,5,null,0
go