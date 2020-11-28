/* This file is used to load the APPLICATION_TEXT table which contains all informational, warnings 
    ,error messages and other soft text that are displayed to the user  
    -- APPLICATION_TEXT 
	-- 	TEXT_CODE : key
	-- 	TEXT_VALUE : string
	-- 	TEXT_MAX_LENGTH : maximum length of string
	-- 	TEXT_TYPE : Used to group types of text for list boxes, etc; Must match eMIDTextType in Enums.cs 
	-- 	TEXT_LEVEL : Used to control how text is handled by the system;  Must match eMIDMessageLevel in Enums.cs
			-- None					= 0
			-- Debug				= 1
			-- Information			= 2
			-- NothingToDo			= 3
			-- Edit					= 4
			-- Warning				= 5
			-- Error				= 6
			-- Severe				= 7
			-- ListBoxItems			= 12
			-- WindowCaption		= 13
			-- Treeview				= 14
			-- Cancelled			= 99
	-- 	TEXT_VALUE_TYPE : Used to identify the type of data associated with a label;  Must match eMIDTextValueType in Enums.cs
	-- 	TEXT_ORDER : Used to specify the order of the text for drop down lists, etc				   */

 
/* truncate table APPLICATION_TEXT */
exec SP_MID_SET_TEXT 000000,'UnAssigned',30,0,null,null,0
/* eMIDMessageLevel */
exec SP_MID_SET_TEXT 000001,'Debug',30,16,null,null,0
exec SP_MID_SET_TEXT 000002,'Information',30,16,null,null,0
exec SP_MID_SET_TEXT 000003,'No Input',30,16,null,null,0
exec SP_MID_SET_TEXT 000004,'Edit',30,16,null,null,0
exec SP_MID_SET_TEXT 000005,'Warning',30,16,null,null,0
exec SP_MID_SET_TEXT 000006,'Error',30,16,null,null,0
exec SP_MID_SET_TEXT 000007,'Severe',30,16,null,null,0
/* Begin TT#1127 - JSmith - Severe error didn't roll up to Scheduler Service */
exec SP_MID_SET_TEXT 00010,'Ignore',30,16,null,null,0
/*End TT#1127  */
exec SP_MID_SET_TEXT 00020,'Unavailable',30,0,null,null,0	/* TT#1581-MD -stodd - Header Reconcile. */
exec SP_MID_SET_TEXT 000099,'Cancelled',30,0,null,null,0
/* exec SP_MID_SET_TEXT 000007,'UI OK',30,16,null,null,0
exec SP_MID_SET_TEXT 000008,'UI OK/Cancel',30,16,null,null,0
exec SP_MID_SET_TEXT 000009,'UI Retry/Cancel',30,16,null,null,0
exec SP_MID_SET_TEXT 000010,'UI Yes/No',30,16,null,null,0
exec SP_MID_SET_TEXT 000011,'UI Yes/No/Cancel',30,16,null,null,0
exec SP_MID_SET_TEXT 000012,'UI Listbox',30,16,null,null,0
exec SP_MID_SET_TEXT 000013,'UI Window Caption',30,16,null,null,0 */

/* Summary text 000100-009999 */
exec SP_MID_SET_TEXT 000100,'Successful completion',30,0,2,null,0
exec SP_MID_SET_TEXT 000101,'Process failed',30,0,5,null,0
exec SP_MID_SET_TEXT 000102,'Process running',30,0,5,null,0
exec SP_MID_SET_TEXT 000103,'Input file not found',30,0,3,null,0
exec SP_MID_SET_TEXT 000104,'Invalid number of parameters specified.',30,0,5,null,0
exec SP_MID_SET_TEXT 000105,'Unexpected Termination',30,0,5,null,0

/* Database Messages 010000-019999 */

/* Services Messages 020000-029999 */
exec SP_MID_SET_TEXT 020000,'System Error',30,0,6,null,0
exec SP_MID_SET_TEXT 020001,'Can not access application server.  ',50,0,6,null,0
exec SP_MID_SET_TEXT 020002,'Bad or missing application config file: ',50,0,7,null,0
exec SP_MID_SET_TEXT 020003,'User Login Failed!',50,0,2,null,0
exec SP_MID_SET_TEXT 020004,'Control Server not found - processing terminated',50,0,2,null,0
exec SP_MID_SET_TEXT 020005,'Client Server not found - processing terminated',50,0,2,null,0
exec SP_MID_SET_TEXT 020006,'Store Server not found - processing terminated',50,0,2,null,0
exec SP_MID_SET_TEXT 020007,'Hierarchy Server not found - processing terminated',50,0,2,null,0
exec SP_MID_SET_TEXT 020008,'Application Server not found - processing terminated',50,0,2,null,0
exec SP_MID_SET_TEXT 020009,'UserRID not initialized',50,0,2,null,0
exec SP_MID_SET_TEXT 020010,'MyHierarchyName not initialized',50,0,2,null,0
exec SP_MID_SET_TEXT 020011,'MyHierarchyColor not initialized',50,0,2,null,0
exec SP_MID_SET_TEXT 020012,'MyWorkflowMethods not initialized',50,0,2,null,0
exec SP_MID_SET_TEXT 020013,'Started building global area.',50,0,2,null,0
exec SP_MID_SET_TEXT 020014,'Finished building global area.',50,0,2,null,0
exec SP_MID_SET_TEXT 020015,'Scheduler Server not found - processing terminated',50,0,2,null,0
exec SP_MID_SET_TEXT 020016,'The database in one or more services does not match client database - processing terminated (Contact application administrator to review application server event viewer)',150,0,7,null,0
exec SP_MID_SET_TEXT 020017,'Scheduler Server could not be found.  Scheduler functions will not be available.  Continue?',150,0,2,null,0
exec SP_MID_SET_TEXT 020018,'Header Server not found - processing terminated',50,0,2,null,0
exec SP_MID_SET_TEXT 020019,'All services must be restarted to activate changes',50,0,5,null,0
exec SP_MID_SET_TEXT 020028,'The version and start information in one or more service does not match application database - processing terminated (Contact application administrator to review application server event viewer)',150,0,7,null,0
exec SP_MID_SET_TEXT 020029,'The configuration information for the client does not match the services - processing terminated (Contact application administrator to review application server event viewer)',150,0,7,null,0
exec SP_MID_SET_TEXT 020030,'The client is incompatible with the version of the services - processing terminated (Contact application administrator to review application server event viewer)',150,0,7,null,0
exec SP_MID_SET_TEXT 020031,'The client is incompatible with the upgrade version of the application database - processing terminated (Contact application administrator to review application server event viewer)',150,0,7,null,0

/* MIDReaderWriterLock Messages 025000-029999 */

exec SP_MID_SET_TEXT 025000,'Attempting unlock action when no actions are pending.',50,0,7,null,0
exec SP_MID_SET_TEXT 025001,'Attempting invalid writer unlock.',50,0,7,null,0
exec SP_MID_SET_TEXT 025002,'Attempting invalid reader unlock.',50,0,7,null,0

/* Calendar Messages 030000-038999 */
exec SP_MID_SET_TEXT 030000,'Calendar Year not found in Calendar.',250,0,5,null,0
exec SP_MID_SET_TEXT 030001,'Calendar Period not found in Calendar.',250,0,5,null,0
exec SP_MID_SET_TEXT 030002,'Calendar Week not found in Calendar.',250,0,5,null,0
exec SP_MID_SET_TEXT 030003,'Invalid Calendar Date Type.',250,0,5,null,0
exec SP_MID_SET_TEXT 030004,'No relative date range profile found.',250,0,5,null,0
exec SP_MID_SET_TEXT 030005,'Calendar Day not found in Calendar.',250,0,5,null,0
exec SP_MID_SET_TEXT 030006,'Invalid Calendar Date.',250,0,5,null,0
exec SP_MID_SET_TEXT 030007,'Posting Date not set in Calendar. Unable to retrieve requested date.',250,0,5,null,0
exec SP_MID_SET_TEXT 030008,'Invalid or missing date range property.',250,0,5,null,0
exec SP_MID_SET_TEXT 030009,'Changing date will impact previously saved calendar date info such as dynamic date definitions and fiscal date relationships.',250,0,5,null,0
exec SP_MID_SET_TEXT 030010,'This will replace the current period definition with a 4-5-4 period definition. Do you wish to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 030011,'There is already a Calendar Model defined with this start. Please choose a different start year.',250,0,6,null,0
exec SP_MID_SET_TEXT 030012,'Calendar model start fiscal year is outside of current system boundaries. Please change to a more recent year.',250,0,6,null,0
exec SP_MID_SET_TEXT 030013,'The number of Weeks in year do not total 52. Currently a model year is required to contain 52 weeks. If a 53 week year is desired, use the 53 Week Year Definition above to designate what years in the model you wish to contain 53 weeks.',250,0,6,null,0
exec SP_MID_SET_TEXT 030014,'Undefined Calendar Date Range.',250,0,6,null,0
exec SP_MID_SET_TEXT 030015,'Date range changes based upon a Relative-To date.',250,0,2,null,0
exec SP_MID_SET_TEXT 030016,'Date range remains static.',250,0,2,null,0
exec SP_MID_SET_TEXT 030017,'While current week is prior to range, acts as a static range. Once current week > start of range, becomes Dynamic to Current.',250,0,2,null,0
exec SP_MID_SET_TEXT 030018,'Same date range every year.',250,0,2,null,0
exec SP_MID_SET_TEXT 030019,'Invalid Parameters sent to {0} method. Parm1: {1}  Parm2: {2}',250,0,6,null,0
exec SP_MID_SET_TEXT 030020,'Select the week you wish the date range to switch to dynamic.',250,0,2,null,0
exec SP_MID_SET_TEXT 030021,'This will replace the current month definition with a 4-5-4 month definition. Do you wish to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 030022,'This will replace the current quarter definition with a 4 quarter definition. Do you wish to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 030023,'This will replace the current season definition with a 2 season definition. Do you wish to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 030024,'No Seasons have been defined.',250,0,6,null,0
exec SP_MID_SET_TEXT 030025,'No Quarters have been defined.',250,0,6,null,0
exec SP_MID_SET_TEXT 030026,'Total number of Quarters defined do not match the number of Quarters used in the Season Definitions.',250,0,6,null,0
exec SP_MID_SET_TEXT 030027,'Total number of Months defined do not match the number of Months used in the Quarter Definitions.',250,0,6,null,0
exec SP_MID_SET_TEXT 030028,'{0} days dyn to Curr.',250,0,2,null,0
exec SP_MID_SET_TEXT 030029,'{0} days dyn to Plan.',250,0,2,null,0
exec SP_MID_SET_TEXT 030030,'{0} days dyn to Str Open.',250,0,2,null,0
exec SP_MID_SET_TEXT 030031,'{0} wks dyn to Curr.',250,0,2,null,0
exec SP_MID_SET_TEXT 030032,'{0} wks dyn to Plan Date.',250,0,2,null,0
exec SP_MID_SET_TEXT 030033,'{0} wks dyn to Str Open.',250,0,2,null,0
exec SP_MID_SET_TEXT 030034,'{0} pds dyn to Curr.',250,0,2,null,0
exec SP_MID_SET_TEXT 030035,'{0} pds dyn to Plan Date.',250,0,2,null,0
exec SP_MID_SET_TEXT 030036,'{0} pds dyn to Str Open.',250,0,2,null,0
exec SP_MID_SET_TEXT 030037,'Period profile key of {0} is invalid.',250,0,6,null,0	/* Track #6018 stodd */
exec SP_MID_SET_TEXT 030038,'Date {0} is not within the range of the merchandise calendar.  The value will be replaced with an undefined date.',250,0,4,null,0	/* TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries */
exec SP_MID_SET_TEXT 030039,'Only 1 week is required for this operator.',250,0,4,null,0   /* TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only */

/* Size Group Messages 039000-039999 */
exec SP_MID_SET_TEXT 039000,'Error: size/dimension labels are not unique.',250,0,5,null,0
exec SP_MID_SET_TEXT 039001,'Error retrieving size code.',250,0,5,null,0
exec SP_MID_SET_TEXT 039002,'Warning: product category for {0}, {1}, does not match any known product categories',250,0,5,null,0
exec SP_MID_SET_TEXT 039003,'Please select group to delete',250,0,5,null,0
exec SP_MID_SET_TEXT 039004,'Delete size group: {0}?',250,0,5,null,0
exec SP_MID_SET_TEXT 039005,'Please select a product category, or typeover "new..." to create a new category',250,0,5,null,0
exec SP_MID_SET_TEXT 039006,'Please select a size group, or typeover "new..." to create a new group',250,0,5,null,0
exec SP_MID_SET_TEXT 039007,'Cannot add new size code.  Please fill in size header and dimension header if appropriate',250,0,5,null,0
exec SP_MID_SET_TEXT 039008,'Size code {0} not found.  Would you like to add it?',250,0,5,null,0
exec SP_MID_SET_TEXT 039009,'Size code "{0}" has size "{1}", dimension "{2}" and product category "{3}".  Would you like to correct this code?',250,0,5,null,0
exec SP_MID_SET_TEXT 039010,'Select correct size code for "{0}", "{1}"',250,0,5,null,0
exec SP_MID_SET_TEXT 039011,'Duplicate size "{0}" is not allowed.',250,0,5,null,0
exec SP_MID_SET_TEXT 039012,'Duplicate dimension "{0}" is not allowed.',250,0,5,null,0
exec SP_MID_SET_TEXT 039013,'Save changes first?',250,0,5,null,0
exec SP_MID_SET_TEXT 039014,'No Secondary Size',30,0,1,null,0
exec SP_MID_SET_TEXT 039015,'The primary/secondary size already exists in the Product Category: Category {0}, Primary {1}, Secondary {2}',30,0,4,null,0
exec SP_MID_SET_TEXT 039016,'A size group must have at least one size defined',30,0,4,null,0
exec SP_MID_SET_TEXT 039017,'The entry for {0}/{1} is blank.  Do you want to fill it in?',30,0,4,null,0
/*Begin Track #3723 - Jeff Scott - Prevent blank size codes from being saved  */
exec SP_MID_SET_TEXT 039018,'No Size Code found for size "{0}", dimension "{1}" and product category "{2}"',30,0,4,null,0
/*End Track #3723 - Jeff Scott - Prevent blank size codes from being saved  */
/*Begin Track #3607 - Jeff Scott - Duplicate key during save of new Group  */
exec SP_MID_SET_TEXT 039019,'Size group exists: "{0}. Overwrite?',30,0,5,null,0
/*End Track #3607 - Jeff Scott - Duplicate key during save of new Group  */

/* General Edit Messages 040000-049999 */
exec SP_MID_SET_TEXT 040000,'Date is required',250,0,4,null,0
exec SP_MID_SET_TEXT 040001,'Date not found',250,0,4,null,0
exec SP_MID_SET_TEXT 040002,'Version is required',250,0,6,null,0
exec SP_MID_SET_TEXT 040003,'Version not found',250,0,6,null,0
exec SP_MID_SET_TEXT 040004,'Input File not found',250,0,3,null,0
exec SP_MID_SET_TEXT 040005,'There is nothing to paste',250,0,5,null,0
exec SP_MID_SET_TEXT 040006,'Unknown-format detected, cannot paste',250,0,5,null,0
exec SP_MID_SET_TEXT 040007,'Do you want to save the changes before continuing?',250,0,2,null,0
exec SP_MID_SET_TEXT 040008,'Search complete.  Unable to find:',250,0,5,null,0
exec SP_MID_SET_TEXT 040009,'Name is required to save',250,0,5,null,0
exec SP_MID_SET_TEXT 040010,'Save canceled',250,0,5,null,0
exec SP_MID_SET_TEXT 040011,'Must be numeric',250,0,4,null,0
exec SP_MID_SET_TEXT 040012,'Field is required',250,0,5,null,0
exec SP_MID_SET_TEXT 040013,'Name already exists.  Do you want to replace?',250,0,5,null,0
exec SP_MID_SET_TEXT 040014,'Total must equal 100',250,0,4,null,0
exec SP_MID_SET_TEXT 040015,'The following errors occurred:',250,0,6,null,0
exec SP_MID_SET_TEXT 040016,'Must be an integer',250,0,4,null,0
exec SP_MID_SET_TEXT 040017,'Clipboard contains the wrong type of data',250,0,6,null,0
exec SP_MID_SET_TEXT 040018,'The delete failed',250,0,6,null,0
exec SP_MID_SET_TEXT 040019,'Copy complete',250,0,2,null,0
exec SP_MID_SET_TEXT 040020,'Delete complete',250,0,2,null,0
exec SP_MID_SET_TEXT 040021,'Copying {0} of {1}',250,0,2,null,0
exec SP_MID_SET_TEXT 040022,'Deleting {0} of {1}',250,0,2,null,0
exec SP_MID_SET_TEXT 040023,'Copying',250,0,2,null,0
exec SP_MID_SET_TEXT 040024,'Deleting',250,0,2,null,0
exec SP_MID_SET_TEXT 040025,'Percent complete:',250,0,2,null,0
exec SP_MID_SET_TEXT 040026,'Error deleting {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 040027,'Do you want to continue?',250,0,2,null,0
exec SP_MID_SET_TEXT 040028,'Delete cancelled',250,0,2,null,0
exec SP_MID_SET_TEXT 040029,'Copy cancelled',250,0,2,null,0
exec SP_MID_SET_TEXT 040030,'All nodes not deleted.  Deleted {0} of {1}.',250,0,6,null,0
exec SP_MID_SET_TEXT 040031,'All nodes not copied.  Copied {0} of {1}.',250,0,6,null,0
exec SP_MID_SET_TEXT 040032,'Application error.  Form not found.',250,0,6,null,0
exec SP_MID_SET_TEXT 040033,'Value exceeds maximum.',250,0,4,null,0
exec SP_MID_SET_TEXT 040034,'Value must be non-negative.',250,0,4,null,0
exec SP_MID_SET_TEXT 040035,'Please select the row where to begin the search.',250,0,2,null,0
exec SP_MID_SET_TEXT 040036,'Column {0} is not in the search area.  Please make sure the correct area is selected.',250,0,5,null,0
exec SP_MID_SET_TEXT 040037,'Column {0} can not be applied.',250,0,5,null,0
exec SP_MID_SET_TEXT 040038,'Format invalid.',250,0,4,null,0
exec SP_MID_SET_TEXT 040039,'You must select a row before executing the operation.',250,0,5,null,0
exec SP_MID_SET_TEXT 040040,'new...',10,0,2,null,0
exec SP_MID_SET_TEXT 040041,'Applying',250,0,2,null,0
exec SP_MID_SET_TEXT 040042,'Applying {0} of {1}',250,0,2,null,0
exec SP_MID_SET_TEXT 040043,'Changes have been successfully applied to all children.',250,0,2,null,0
exec SP_MID_SET_TEXT 040044,'Value must be unique.',250,0,4,null,0
exec SP_MID_SET_TEXT 040045,'Application error: Not all values successfully spread.',250,0,6,null,0
exec SP_MID_SET_TEXT 040046,'Data is Read Only.  Changes will not be saved.',250,0,5,null,0
exec SP_MID_SET_TEXT 040047,'Errors were found.  Please review and correct.',250,0,6,null,0
exec SP_MID_SET_TEXT 040048,'You are not authorized to perform the requested action.',250,0,5,null,0
exec SP_MID_SET_TEXT 040049,'You are not authorized to perform the requested action for this item.',250,0,5,null,0
exec SP_MID_SET_TEXT 040050,'Data is being locked for deletion',250,0,2,null,0
exec SP_MID_SET_TEXT 040051,'End must be greater than begin',250,0,4,null,0
exec SP_MID_SET_TEXT 040052,'Noncomp dates must be greater than new store dates',250,0,4,null,0
exec SP_MID_SET_TEXT 040053,'Must be positive integer.',250,0,4,null,0
exec SP_MID_SET_TEXT 040054,'Entered value does not match the field data type. Value will be ignored',250,0,4,null,0
exec SP_MID_SET_TEXT 040055,'Input is invalid',250,0,4,null,0
exec SP_MID_SET_TEXT 040056,'Delete Filter {0}?',250,0,2,null,0
/*Begin TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed */
/*exec SP_MID_SET_TEXT 040057,'Outdated filter information found -- some parameters have been omitted',250,0,5,null,0*/
exec SP_MID_SET_TEXT 040057,'Outdated filter information found -- filter will not be applied',250,0,5,null,0
/*End TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed */
exec SP_MID_SET_TEXT 040058,'Unbalanced Parenthesis',250,0,5,null,0
exec SP_MID_SET_TEXT 040059,'Attribute Set or Store expected',250,0,5,null,0
exec SP_MID_SET_TEXT 040060,'Unmatched Right Parenthesis',250,0,5,null,0
exec SP_MID_SET_TEXT 040061,'AND or OR expected',250,0,5,null,0
exec SP_MID_SET_TEXT 040062,'=, <, >, <=, or >= expected',250,0,5,null,0
exec SP_MID_SET_TEXT 040070,'Variable or quantity expected',250,0,5,null,0
exec SP_MID_SET_TEXT 040071,'Variable expected',250,0,5,null,0
exec SP_MID_SET_TEXT 040072,'Unexpected End Of Line',250,0,5,null,0
exec SP_MID_SET_TEXT 040073,'No Sets assigned',250,0,5,null,0
exec SP_MID_SET_TEXT 040074,'No Stores assigned',250,0,5,null,0
exec SP_MID_SET_TEXT 040075,'Quantity has not been assigned',250,0,5,null,0
exec SP_MID_SET_TEXT 040076,'Allocation Header ID is Required',250,0,5,null,0
exec SP_MID_SET_TEXT 040077,'Allocation Header ID is NOT Allowed',250,0,5,null,0
exec SP_MID_SET_TEXT 040078,'DateRange has not been assigned',250,0,5,null,0
exec SP_MID_SET_TEXT 040079,'"Corresponding" not valid for this variable',250,0,5,null,0
exec SP_MID_SET_TEXT 040080,'At least {0} {1} entries are required',250,0,7,null,0 
exec SP_MID_SET_TEXT 040081,'Are you sure you want to delete {0}?',250,0,2,null,0
exec SP_MID_SET_TEXT 040082,'Are you sure you want to remove {0}?',250,0,2,null,0
exec SP_MID_SET_TEXT 040083,'Remove cancelled',250,0,2,null,0
exec SP_MID_SET_TEXT 040084,'This function requires MS Excel Version 9.0 or higher.  Please check your configuration.',250,0,2,null,0
exec SP_MID_SET_TEXT 040085,'Must be between 0 and 100.',250,0,2,null,0
exec SP_MID_SET_TEXT 040086,'Value cannot exceed 100.',250,0,2,null,0
exec SP_MID_SET_TEXT 040087,'Filter has not been saved.  Are you sure you want to quit?',250,0,2,null,0
exec SP_MID_SET_TEXT 040088,'{0} is required.',250,0,2,null,0
exec SP_MID_SET_TEXT 040089,'{0} occurrance(s) replaced.',250,0,2,null,0
exec SP_MID_SET_TEXT 040090,'Please specify the text to be replaced.',250,0,2,null,0
exec SP_MID_SET_TEXT 040091,'Grade has not been assigned',250,0,5,null,0
exec SP_MID_SET_TEXT 040092,'Status has not been assigned',250,0,5,null,0
exec SP_MID_SET_TEXT 040093,'Quantity expected',250,0,5,null,0
exec SP_MID_SET_TEXT 040094,'Variable or Grade expected',250,0,5,null,0
exec SP_MID_SET_TEXT 040095,'Variable or Status expected',250,0,5,null,0
exec SP_MID_SET_TEXT 040096,'Unable to delete because data is being referenced',250,0,5,null,0
exec SP_MID_SET_TEXT 040097,'Removing this value will reestablish inheritance upon successful save.  Do you want to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 040098,'Smaller subsequent values found',250,0,5,null,0
exec SP_MID_SET_TEXT 040099,'New Store Period End must be blank if New store Period Begin is blank',250,0,5,null,0
exec SP_MID_SET_TEXT 040100,'New Store Period End cannont be blank unless Non-Comp Perioid Begin is non-blank or New Store Period Begin is negative',250,0,5,null,0
exec SP_MID_SET_TEXT 040101,'Merchandise is required',250,0,5,null,0
exec SP_MID_SET_TEXT 040102,'Method is required',250,0,5,null,0
exec SP_MID_SET_TEXT 040103,'{0} - {1}  Method is marked as invalid.',250,0,4,null,0
exec SP_MID_SET_TEXT 040104,'The Header previously used as a basis for this rule was deleted from the system. The rule has been changed to reflect this.',250,0,2,null,0
exec SP_MID_SET_TEXT 040105,'The selected Method is currently marked as invalid. The method must be corrected before it can be used in a Workflow.',250,0,4,null,0
exec SP_MID_SET_TEXT 040106,'Value exceeds multiple.',250,0,4,null,0
exec SP_MID_SET_TEXT 040107,'Workflow is required',250,0,5,null,0
exec SP_MID_SET_TEXT 040108,'Unable to determine where to add row; please select a valid row before adding new row',250,0,5,null,0
exec SP_MID_SET_TEXT 040109,'Merchandise is invalid',250,0,5,null,0
exec SP_MID_SET_TEXT 040110,'Selecting this option may affect performance.',250,0,5,null,0
/*UPDATED 01-19-05 John Smith - add confirmation messages */
exec SP_MID_SET_TEXT 040111,'Do you want to move {0} to {1}?',250,0,5,null,0
exec SP_MID_SET_TEXT 040112,'Do you want to copy {0} to {1}?',250,0,5,null,0
exec SP_MID_SET_TEXT 040113,'Do you want to make a reference to {0} under {1}?',250,0,5,null,0
exec SP_MID_SET_TEXT 040114,'The combination of product category {0}, primary {1}, and secondary {2} already exists for size code {3}',250,0,5,null,0
exec SP_MID_SET_TEXT 040115,'Primary is required to define a size',250,0,5,null,0
exec SP_MID_SET_TEXT 040116,'Product Level Delimiter is required and must be a single character',250,0,5,null,0
exec SP_MID_SET_TEXT 040117,'Workflow failed.  Tolerance for {0} exceeded.',250,0,5,null,0
exec SP_MID_SET_TEXT 040118,'{0} is installed.',250,0,5,null,0
exec SP_MID_SET_TEXT 040119,'{0} is not installed.',250,0,5,null,0
exec SP_MID_SET_TEXT 040120,'{0} expires in {1} days.',250,0,5,null,0
exec SP_MID_SET_TEXT 040121,'Please select an entry.',250,0,5,null,0
exec SP_MID_SET_TEXT 040122,'Code is required.',250,0,5,null,0
/*Begin Track #3704 - JScott - To Length can be less than From Length on Hierarchy Properties */
exec SP_MID_SET_TEXT 040123,'To Length must be greater or equal to From Length.',250,0,5,null,0
/*End Track #3704 - JScott - To Length can be less than From Length on Hierarchy Properties */
/*Begin Assortment Planning - JSmith - Multi select */
exec SP_MID_SET_TEXT 040124,'A multi-selection with the SHIFT key must all have the same parent',250,0,5,null,0
/*Begin Track #4010 - JScott - Add all forecast methods and workflows to the scheduler. */
exec SP_MID_SET_TEXT 040125, 'Workflow or Method is required',250,0,6,null,0
/*End Track #4010 - JScott - Add all forecast methods and workflows to the scheduler. */
exec SP_MID_SET_TEXT 040126,'A required basis is missing.',250,0,5,null,0
exec SP_MID_SET_TEXT 040127,'A value is required in either the Index or the units.',250,0,5,null,0
exec SP_MID_SET_TEXT 040128,'A value is allowed in the Index or the units, but not both.',250,0,5,null,0
exec SP_MID_SET_TEXT 040129,'Delete {0} for user {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 040130,'Delete {0} from workflow {1} for user {2}.',250,0,2,null,0
exec SP_MID_SET_TEXT 040131,'Delete {0} from tasklist {1} for user {2}.',250,0,2,null,0
exec SP_MID_SET_TEXT 040132,'Cleaning up explorer.  Please wait...',250,0,2,null,0
/*End Assortment Planning  */
exec SP_MID_SET_TEXT 040133,'To start your search, enter the criteria on the left pane and press Search',250,0,2,null,0
exec SP_MID_SET_TEXT 040134,'Search by any or all of the criteria below',250,0,2,null,0
exec SP_MID_SET_TEXT 040135,'Store grades already exist for this method. Do you wish to replace them with the store grades from the {0}?',250,0,5,null,0
exec SP_MID_SET_TEXT 040136,'Units grade boundary is selected, but no units have been entered.',250,0,6,null,0
exec SP_MID_SET_TEXT 040137,'Index grade boundary is selected, but no indexes have been entered.',250,0,6,null,0
exec SP_MID_SET_TEXT 040138,'Characteristic value expected',250,0,5,null,0
exec SP_MID_SET_TEXT 040139,'File name is required',250,0,6,null,0
exec SP_MID_SET_TEXT 040140,'The specified directory does not exist.  Do you want to create it?',250,0,5,null,0
exec SP_MID_SET_TEXT 040141,'The file {0} already exists.  Do you want to replace the existing file?',250,0,5,null,0
exec SP_MID_SET_TEXT 040142,'Sell Thru Percents already exist for this method. Do you wish to replace them with the Sell Thru Percents from the merchandise?',250,0,5,null,0
/* BEGIN MID Track #4370 - John Smith - FWOS Models */
exec SP_MID_SET_TEXT 040143,'Unable to balance. The maximum number of attempts ({0}) has been exceeded',250,0,5,null,0
/* END MID Track #4370 */

exec SP_MID_SET_TEXT 040144,'Maximum must be greater than or equal to Minimum',250,0,5,null,0
exec SP_MID_SET_TEXT 040145,'Store Grades do not match',250,0,5,null,0
exec SP_MID_SET_TEXT 040146,'Attribute does not match',250,0,5,null,0
exec SP_MID_SET_TEXT 040147,'Only copy the current set?',250,0,5,null,0
/* BEGIN MID Track #4386 - Justin Bolles - Delete Views */
exec SP_MID_SET_TEXT 040148,'Are sure you want to delete all user views?',250,0,5,null,0
/* Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting */
exec SP_MID_SET_TEXT 040149,'Computation model not found.  Computation requests will not be generated.',250,0,5,null,0
/* End - Abercrombie & Fitch #4411 */
exec SP_MID_SET_TEXT 040150,'View Deleted Successfully.',250,0,2,null,0
/* END MID Track #4386 */
/* Begin Track #4457 - JSmith - Add forecast versions */
exec SP_MID_SET_TEXT 040151,'Cannot delete row used in a combined version.',250,0,6,null,0
exec SP_MID_SET_TEXT 040152,'Cannot delete a standard version.',250,0,6,null,0
exec SP_MID_SET_TEXT 040153,'Cannot use combined version as forecast version.',250,0,6,null,0
/* End Track #4457 */
/* Begin Track #4619 - JSmith - Add auto-upgrade */
exec SP_MID_SET_TEXT 040154,'Client version is not current. \n   Client={0} \n   Application={1}. \n The client will be terminated.',250,0,6,null,0
/* End Track #4619 */
/* Begin MID Track #4310 - JSmith - Store Grades not defined */
exec SP_MID_SET_TEXT 040155,'Store Grades not found',250,0,6,null,0
/* End Track #4310 */
/* Begin Track #4815 - JSmith - #283-User (Security) Maintenance */
exec SP_MID_SET_TEXT 040156,'Are you sure you want to recover {0}?',250,0,2,null,0
exec SP_MID_SET_TEXT 040157,'{0} has been successfully deleted.',250,0,2,null,0
exec SP_MID_SET_TEXT 040158,'Are you sure you want to assign {0}?',250,0,2,null,0
exec SP_MID_SET_TEXT 040159,'Are you sure you want to unassign {0}?',250,0,2,null,0
exec SP_MID_SET_TEXT 040160,'Shared by {0}',250,0,2,null,0
/* End Track #4815 */
exec SP_MID_SET_TEXT 040167,'[RelieveAllStores] field must be boolean key word "true" or "false"',250,0,2,null,0 /* MID Track 5694 MA enhancement Relieve IT by Header ID */
exec SP_MID_SET_TEXT 040168,'[RelieveAllStores] field cannot be both "true" and "false" for same header: ',250,0,2,null,0 /* MID Track 5694 MA enhancement Relieve IT by Header ID */
exec SP_MID_SET_TEXT 040169,'[RelieveAllStores] field cannot be specified when Headers are not specified. ',250,0,2,null,0 /* MID Track 5694 MA enhancement Relieve IT by Header ID */
exec SP_MID_SET_TEXT 040170,'[RelieveAllStores] field overridden for header [{0}] to value: [{1}]',250,0,2,null,0 /* MID Track 5694 MA enhancement Relieve IT by Header ID */
/* Track #5739 – KJohnson - Product Characteristic dragging problem */
exec SP_MID_SET_TEXT 040171,'Must click apply before draging this item.',250,0,2,null,0 /* Track #5739 – KJohnson - Product Characteristic dragging problem */
/* End Track #5739 */
/* Track #4872 - JSmith - Global/User Attributes */
exec SP_MID_SET_TEXT 040172,'You no longer have access to attribute {0} from user {1}.  Do you want to change it?  \n Warning: Changing the attribute may remove settings.',250,0,5,null,0 
/* End Track #4872 */
exec SP_MID_SET_TEXT 040173,'Variable has already been added to this list.',250,0,2,null,0 /* MID Track #5773 - KJohnson - Planned FWOS Enhancement */
/* Track #6047 – KJohnson – New message */
exec SP_MID_SET_TEXT 040174,'Weight cannot be zero or negative.',250,0,2,null,0
/* End Track #6047 */
/* Begin Track #5663 - JScott - Export to Excel when I type in the path to export to received and unauthorizedAccessException. */
exec SP_MID_SET_TEXT 040175,'The file name specified is an existing directory.  Please specify a file name.',250,0,5,null,0 
/* End Track #5663 - JScott - Export to Excel when I type in the path to export to received and unauthorizedAccessException. */
/* BEGIN MID Track #6336 - Header Load API Enhancement */
exec SP_MID_SET_TEXT 040176,'Allocation Header [{0}] has Method [{1}] with NO Method Type specified.',250,0,5,null,0
exec SP_MID_SET_TEXT 040177,'Allocation Header [{0}] has Method [{1}] with an invalid Method Type [{2}] specified.',250,0,5,null,0
exec SP_MID_SET_TEXT 040178,'Allocation Header [{0}] has both a Workflow [{1}] and Method(s) defined. Both are not allowed.',250,0,5,null,0
exec SP_MID_SET_TEXT 040179,'Allocation Header [{0}] with Workflow [{1}] does not exist.',250,0,5,null,0
exec SP_MID_SET_TEXT 040180,'Global method name [{0}] with method type [{1}] does not exist.',250,0,5,null,0
exec SP_MID_SET_TEXT 040181,'Allocation Header [{0}] has Method Type [{1}] with an invalid Action Name [{2}] specified.',250,0,5,null,0
/* END MID Track #6336 */

exec SP_MID_SET_TEXT 040161,'This item cannot be checked when the From and To levels are equal.',250,0,2,null,0
exec SP_MID_SET_TEXT 040162,'Either this item or any combination of other items must be checked for processing to work.',250,0,2,null,0
exec SP_MID_SET_TEXT 040163,'Item is not valid; please enter or select a valid item.',250,0,4,null,0
exec SP_MID_SET_TEXT 040164,'You are not authorized for this item.',250,0,4,null,0
exec SP_MID_SET_TEXT 040165,'Name already exist. Please select a different name to continue.',250,0,2,null,0
exec SP_MID_SET_TEXT 040166,'Can not set override model to a non-custom one when the method calling this override is set to a custom model to begin with.',250,0,2,null,0
exec SP_MID_SET_TEXT 040182,'You must select at least one option.',250,0,2,null,0
/* Begin TT#155 - JSmith - Size Curve Method */
exec SP_MID_SET_TEXT 040183,'You must select at least one basis.',250,0,2,null,0
/* End TT#155 */
exec SP_MID_SET_TEXT 040184,'In OPTIONS, Characteristic Auto Add value is invalid. Value is {0}.',250,0,4,null,0
exec SP_MID_SET_TEXT 040185,'In OPTIONS, Characteristic Type Delimiter can only be a single character. Delimiter value is {0}.',250,0,4,null,0
exec SP_MID_SET_TEXT 040186,'Invalid Characteristic Type for auto-add characteristic, {0}. Char type = {1}',250,0,4,null,0
exec SP_MID_SET_TEXT 040187,'Characteristic, {0}, was auto added.',250,0,2,null,0
exec SP_MID_SET_TEXT 040188,'You must enter a value in the format "Attribute name/Attribute Set name" or use drag/drop',250,0,2,null,0
exec SP_MID_SET_TEXT 040189,'Attribute name does not exist',250,0,2,null,0
exec SP_MID_SET_TEXT 040190,'Attribute Set name does not exist',250,0,2,null,0
/* Begin TT#274 - RMatelic - Purge Criteria report issues */
exec SP_MID_SET_TEXT 040191,'You must enter a valid value for {0}',250,0,6,null,0 
/* End TT#274  */        
/* Begin TT#384 - JSmith - Error removing child from hierarchy */
exec SP_MID_SET_TEXT 040192,'The information on the explorer is out of date and will be refreshed.',250,0,5,null,0
/* End TT#384 */
/* Begin TT#703 - Stodd - export method */
exec SP_MID_SET_TEXT 040193,'Path: {0} could not be created. You may not have write authority to all or part of the path',250,0,5,null,0
/* End TT#703 */
/* Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model */
exec SP_MID_SET_TEXT 040194,'Selected criteria will display {0} items. Do you want to display all items?',250,0,2,null,0
/* End TT#988 */
/* BEGIN TT#1401 - stodd - add resevation stores (IMO) */
exec SP_MID_SET_TEXT 040195,'In OPTIONS, Use Characteristic Transaction indicator is invalid. Should be ''True'', ''False'', or left blank. Value is {0}.',250,0,4,null,0
exec SP_MID_SET_TEXT 040196,'Header characteristic, {0}, does not exist. This may be a mismatch between the config setting and the name of the header characteristic.',250,0,7,null,0
exec SP_MID_SET_TEXT 040197,'Could not convert {0} to a valid date time for header {1}.',250,0,6,null,0
exec SP_MID_SET_TEXT 040198,'Header {0} skipped. Header already in Released or Released Approved status.',250,0,2,null,0
exec SP_MID_SET_TEXT 040199,'Header {0} skipped. Header not in Received in Balance status.',250,0,2,null,0
/*Begin TT#1904- DOConnell - Filter Error */
exec SP_MID_SET_TEXT 040201,'View Name exceeds maximum length of  {0} \n Please choose a shorter name.',250,0,2,null,0
/*End TT#1904- DOConnell - Filter Error*/
exec SP_MID_SET_TEXT 040209,'Header {0} Processed Force Flag: {1}. First Store Ack Date: {2}. Push to Back Stock Num Weeks: {3}. Push to Back Stock Date: {4}. Posting Date: {5}.',250,0,2,null,0
exec SP_MID_SET_TEXT 040210,'Header {0} Skipped. Force Flag: {1}. First Store Ack Date: {2}. Push to Back Stock Num Weeks: {3}. Push to Back Stock Date: {4}. Posting Date: {5}.',250,0,2,null,0

/* END TT#1401 - stodd - add resevation stores (IMO) */


/* Begin TT#1921 - JSmith - Error during Hierarchy Reclass  */
exec SP_MID_SET_TEXT 040202,'Value of {0} exceeds the maximum length of {1} for database column {2} on table {3}',250,0,6,null,0
/* End TT#1921 */

/*Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts */
exec SP_MID_SET_TEXT 040203,' All Resources for {1} have been released by {0} ',250,0,2,null,0
exec SP_MID_SET_TEXT 040204,' {2} Layouts for {1} have been cleared by {0} ',250,0,2,null,0
exec SP_MID_SET_TEXT 040205,' Layouts for {1} have been Scheduled to be cleared by {0}.\n Process will be completed when {1} logs out.',250,0,2,null,0
exec SP_MID_SET_TEXT 040206,' User {0} is currently logged on.\n',250,0,2,null,0
exec SP_MID_SET_TEXT 040207,' Processed scheduled clearing of {0} Layouts for {1} ',250,0,2,null,0
exec SP_MID_SET_TEXT 040208,' {0} Layouts successfully cleared. ',250,0,2,null,0
/*End TT#1888 - DOConnell - Add feature to clear Infragistics Layouts */

/* Begin TT#2015 - gtaylor - apply changes to lower levels */
exec SP_MID_SET_TEXT 040211,'Cannot be Applied at this time.',250,0,2,null,0
/* Begin TT#2015 - gtaylor - apply changes to lower levels */     

exec SP_MID_SET_TEXT 040212,'Do you want to apply the changes before continuing?',250,0,2,null,0 -- Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set   
exec SP_MID_SET_TEXT 040213,'Message {0} received from {1} with priority {2}. Processed = {3}.',250,0,2,null,0 -- TT#2307 - JSmith - Incorrect Stock Values   
exec SP_MID_SET_TEXT 040214,'Message listener started for {0}.',250,0,2,null,0 -- TT#2307 - JSmith - Incorrect Stock Values   

exec SP_MID_SET_TEXT 040216,'Total adjusted to 100',250,0,5,null,0 /* TT#280 - MD - DOConnell - Daily Percentages error messages are incorrect */
exec SP_MID_SET_TEXT 040217, 'Is in use by the following:',250,0,2,null,0 /* TT#110-MD - RMatelic - In Use Tool */
exec SP_MID_SET_TEXT 040218,'{0} Explorer folder build error. {1} not found for key:{2}',250,0,6,null,0 /* TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)  */
/* TT#1199 - MD - NEED allocation # of stores is zero – rbeck */
exec SP_MID_SET_TEXT 040219,'All store quantities and the quantity allocated before the action are not equal.',250,0,4,null,0
/* TT#753-754 – rbeck - Log informational message added to audit   */
exec SP_MID_SET_TEXT 040220,'OTS Forecast Method {0} has written the MONITOR to location {1} by user <{2},{3}>',250,0,2,null,0
exec SP_MID_SET_TEXT 040221,'Modify Sales Method {0} has written the MONITOR to location {1} by user <{2},{3}>',250,0,2,null,0
exec SP_MID_SET_TEXT 040222,'{0} empty dynamic store attribute sets have been successfully deleted.',250,0,2,null,0		/* // TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 040223,'You must close all windows before selecting Refresh.',250,0,5,null,0		/* TT#4287 - JSmith - Do not allow refresh if windows are open */

/* Begin TT#1646-MD - jsmith - Add or Remove Hierarchy Levels */
exec SP_MID_SET_TEXT 040224, 'Process completed successfully.',250,0,2,null,0
exec SP_MID_SET_TEXT 040225, 'Process completed with errors.',250,0,6,null,0
exec SP_MID_SET_TEXT 040226, 'Process did not start due to issues found.',250,0,6,null,0
exec SP_MID_SET_TEXT 040227, 'Transactions Processed:',250,0,2,null,0
exec SP_MID_SET_TEXT 040228, '{0} Added:',250,0,2,null,0
exec SP_MID_SET_TEXT 040229, '{0} Removed:',250,0,2,null,0
exec SP_MID_SET_TEXT 040230, 'Errors:',250,0,2,null,0
exec SP_MID_SET_TEXT 040231, '{0} Process NOT Run',250,0,2,null,0
exec SP_MID_SET_TEXT 040232, 'Error encountered during deserialization of the file ''{0}''.',250,0,6,null,0
exec SP_MID_SET_TEXT 040233, 'Error encountered while processing the file ''{0}''.',250,0,6,null,0
exec SP_MID_SET_TEXT 040234, 'Delimiter defined as {0} in CONFIG file.',250,0,6,null,0
exec SP_MID_SET_TEXT 040235, 'Unknown action {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 040236, 'Processing {0}.',250,0,2,null,0
exec SP_MID_SET_TEXT 040237, 'Completed {0}.',250,0,2,null,0
exec SP_MID_SET_TEXT 040238, '{0} executable location: {1}',250,0,2,null,0
exec SP_MID_SET_TEXT 040239, '{0} arguments: {1}',250,0,2,null,0
exec SP_MID_SET_TEXT 040240, 'Error processing {0}. Exit code = {1}',250,0,6,null,0
exec SP_MID_SET_TEXT 040241, 'File location: {0}',250,0,2,null,0
exec SP_MID_SET_TEXT 040242, 'Cannot find file {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 040242, 'Cannot find file {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 040243, 'Configuration setting {0} has the value {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 040244, 'Cannot find setting {0} in configuration file {1}.',250,0,6,null,0
exec SP_MID_SET_TEXT 040245, 'Processing files at {0} with trigger {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 040246, 'There are no files to process at {0} with trigger {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 040247, 'Transaction={0}.',250,0,2,null,0
exec SP_MID_SET_TEXT 040248, '{0} is an empty file.',250,0,2,null,0
exec SP_MID_SET_TEXT 040249, 'Input file not specified.',250,0,6,null,0
exec SP_MID_SET_TEXT 040250, 'Process must be executed on the Logility RO Application Server.',250,0,6,null,0
exec SP_MID_SET_TEXT 040251, 'Input record does not contain the minimum number of expected fields. This normally indicates that the delimiter in the config file does not match the delimiter used in the input file.',250,0,5,null,0
/* End TT#1646-MD - jsmith - Add or Remove Hierarchy Levels */
exec SP_MID_SET_TEXT 040252,'User changes have been applied in Group Allocation and its Headers.  Do you want to save the changes before closing or discard them?',250,0,2,null,0  /* TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway */
exec SP_MID_SET_TEXT 040253,'User changes have been applied in Assortment and its Headers.  Do you want to save the changes before closing or discard them?',250,0,2,null,0  /* TT#1954-MD - AGallagher - Assortment */
exec SP_MID_SET_TEXT 040254,'Begin processing {0} method {1}',250,0,2,null,0  /* TT#1966-MD - JSmith - DC Fulfillment */
exec SP_MID_SET_TEXT 040255,'End processing {0} method {1} successfully',250,0,2,null,0  /* TT#1966-MD - JSmith - DC Fulfillment */
exec SP_MID_SET_TEXT 040256,'End processing {0} method {1} with errors',250,0,2,null,0  /* TT#1966-MD - JSmith - DC Fulfillment */
exec SP_MID_SET_TEXT 040257,'DC Fulfillment Method {0} has written the MONITOR to location {1} by user <{2},{3}>',250,0,2,null,0   /* TT#1966-MD - AGallagher - DC Fulfillment */
exec SP_MID_SET_TEXT 040258,'Will open in Read Only mode.',250,0,2,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 040259,'The selected workflow cannot be deleted at this time.',250,0,2,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 040260,'The selected workflow cannot be updated at this time.',250,0,2,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 040261,'The selected method cannot be deleted at this time.',250,0,2,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 040262,'The selected method cannot be updated at this time.',250,0,2,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 040263,'The following workflow requested',250,0,2,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 040264,'The following method requested',250,0,2,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 040265,'Delete(s) cannot be accomplished due to conflicts.',250,0,5,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 040266,'Call parameters do not match instance values.',250,0,7,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 040267,'Data is not lock and cannot be updated at this time.',250,0,5,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 040268,'Color not found on header.',250,0,5,null,0   /* RO-3444 - JSmith - RO Web Messaging */
        
/* Hierarchy Messages 050000-059999 */
exec SP_MID_SET_TEXT 050000,'Cannot add levels to Alternate hierarchy',250,0,4,null,0
exec SP_MID_SET_TEXT 050001,'Hierarchy color not provided.  Using default',250,0,2,null,0
exec SP_MID_SET_TEXT 050002,'Hierarchy level color not provided.  Using default.',250,0,2,null,0
exec SP_MID_SET_TEXT 050003,'Hierarchy level length type not provided.  Using default.',250,0,2,null,0
exec SP_MID_SET_TEXT 050004,'Hierarchy level name is required.',250,0,4,null,0
exec SP_MID_SET_TEXT 050005,'Hierarchy level range from not provided.  Using zero.',250,0,2,null,0
exec SP_MID_SET_TEXT 050006,'Hierarchy level range to not provided.  Using zero.',250,0,2,null,0
exec SP_MID_SET_TEXT 050007,'Hierarchy level required size not provided.  Using zero.',250,0,2,null,0
exec SP_MID_SET_TEXT 050008,'Hierarchy level type not provided.  Using none.',250,0,2,null,0
exec SP_MID_SET_TEXT 050009,'Hierarchy not found',250,0,4,null,0
exec SP_MID_SET_TEXT 050010,'Hierarchy required',250,0,4,null,0
exec SP_MID_SET_TEXT 050011,'Hierarchy type not provided.  Using alternate',250,0,4,null,0
exec SP_MID_SET_TEXT 050012,'Invalid hierarchy type',250,0,4,null,0
exec SP_MID_SET_TEXT 050013,'Invalid level length type',250,0,4,null,0
exec SP_MID_SET_TEXT 050014,'Invalid level type',250,0,4,null,0
exec SP_MID_SET_TEXT 050015,'Invalid product type',250,0,4,null,0
exec SP_MID_SET_TEXT 050016,'Invalid level range',250,0,4,null,0
exec SP_MID_SET_TEXT 050017,'Invalid level required size',250,0,4,null,0
exec SP_MID_SET_TEXT 050018,'Parent is required',250,0,4,null,0
exec SP_MID_SET_TEXT 050019,'Parent not found',250,0,4,null,0
exec SP_MID_SET_TEXT 050020,'Parent required to auto-add',250,0,4,null,0
exec SP_MID_SET_TEXT 050021,'Product is required',250,0,4,null,0
exec SP_MID_SET_TEXT 050022,'Required size must be greater than zero',250,0,4,null,0
exec SP_MID_SET_TEXT 050023,'Attempted to add beyond defined levels',250,0,5,null,0
exec SP_MID_SET_TEXT 050024,'The first level can not go up',250,0,5,null,0
exec SP_MID_SET_TEXT 050025,'The last level can not go down',250,0,5,null,0
exec SP_MID_SET_TEXT 050026,'Can not remove nested level',250,0,5,null,0
exec SP_MID_SET_TEXT 050027,'Can not paste to nested levels',250,0,5,null,0
exec SP_MID_SET_TEXT 050028,'Node can not be in the same hierarchy twice.  Must be moved.',250,0,5,null,0
exec SP_MID_SET_TEXT 050029,'Node must be at the same level in the hierarchy',250,0,5,null,0
exec SP_MID_SET_TEXT 050030,'Can not drop a node into a shared path',250,0,5,null,0
exec SP_MID_SET_TEXT 050031,'Can not move a node into personal folder',250,0,5,null,0
exec SP_MID_SET_TEXT 050032,'Invalid OTS Forecast Level type',250,0,4,null,0
exec SP_MID_SET_TEXT 050033,'Can not add to guest nodes',250,0,5,null,0
exec SP_MID_SET_TEXT 050034,'Product ID already exists',250,0,5,null,0
exec SP_MID_SET_TEXT 050035,'Must be a hierarchy node to drop here',250,0,5,null,0
exec SP_MID_SET_TEXT 050036,'This level can not be moved',250,0,5,null,0
exec SP_MID_SET_TEXT 050037,'Color must be after style',250,0,5,null,0
exec SP_MID_SET_TEXT 050038,'Size must be lowest level',250,0,5,null,0
exec SP_MID_SET_TEXT 050039,'This level can not be moved below the style level',250,0,5,null,0
exec SP_MID_SET_TEXT 050040,'A level of this type is already defined',250,0,5,null,0
exec SP_MID_SET_TEXT 050041,'Can not rename nodes with complex display options',250,0,5,null,0
exec SP_MID_SET_TEXT 050042,'Color code is not on file.  Do you wish to add this code?',250,0,2,null,0
exec SP_MID_SET_TEXT 050043,'Product is not on file.',250,0,4,null,0
exec SP_MID_SET_TEXT 050044,'Level can not be added below style.',250,0,5,null,0
exec SP_MID_SET_TEXT 050045,'Hierarchy already exists.',250,0,4,null,0
exec SP_MID_SET_TEXT 050046,'Can not delete nodes with children.',250,0,5,null,0
exec SP_MID_SET_TEXT 050047,'Levels will be delete',250,0,2,null,0
exec SP_MID_SET_TEXT 050048,'The format of the node is invalid',250,0,4,null,0
exec SP_MID_SET_TEXT 050049,'The first level must be a style',250,0,5,null,0
exec SP_MID_SET_TEXT 050050,'This node can not be moved',250,0,5,null,0
exec SP_MID_SET_TEXT 050051,'Can not copy or move from My Hierarchies to Organizational or Alternate hierarchies',250,0,5,null,0
exec SP_MID_SET_TEXT 050052,'Can not copy or move from Alternate hierarchy to Organizational hierarchy',250,0,6,null,0
exec SP_MID_SET_TEXT 050053,'This folder can not be renamed',250,0,6,null,0
exec SP_MID_SET_TEXT 050054,'Nodes can only be added to hierarchies or other nodes',250,0,6,null,0
exec SP_MID_SET_TEXT 050055,'Nodes can not be dropped on root folders',250,0,6,null,0
exec SP_MID_SET_TEXT 050056,'This node can not be copied',250,0,6,null,0
exec SP_MID_SET_TEXT 050057,'This ID is already defined to another node',250,0,6,null,0
exec SP_MID_SET_TEXT 050058,'ID is required',250,0,4,null,0
exec SP_MID_SET_TEXT 050059,'Description is required',250,0,4,null,0
exec SP_MID_SET_TEXT 050060,'Name is required',250,0,4,null,0
exec SP_MID_SET_TEXT 050061,'Properties are not available for this node',250,0,5,null,0
exec SP_MID_SET_TEXT 050062,'Do you want to copy all descendants for this node?',250,0,2,null,0
exec SP_MID_SET_TEXT 050063,'Can not add nodes beyond hierarchy definition',250,0,6,null,0
exec SP_MID_SET_TEXT 050064,'Only one organizational hierarchy can be defined',250,0,4,null,0
exec SP_MID_SET_TEXT 050065,'Node already a member of parent node',250,0,5,null,0
exec SP_MID_SET_TEXT 050066,'Levels do not match. Operation can not be completed',250,0,6,null,0
exec SP_MID_SET_TEXT 050067,'Can not insert level because there are nodes defined at the next level in the hierarchy.',250,0,5,null,0
exec SP_MID_SET_TEXT 050068,'Can not delete level because there are nodes defined at this level.',250,0,5,null,0
exec SP_MID_SET_TEXT 050069,'Can not delete node from shared path.',250,0,6,null,0
exec SP_MID_SET_TEXT 050070,'Number of descendants:',250,0,2,null,0
exec SP_MID_SET_TEXT 050071,'Are you sure you want to delete {0} and all descendants?',250,0,2,null,0
exec SP_MID_SET_TEXT 050072,'Are you sure you want to delete {0}?',250,0,2,null,0
exec SP_MID_SET_TEXT 050073,'Can not copy color or size to the same parent.  Must add new node.',250,0,5,null,0
exec SP_MID_SET_TEXT 050074,'Node ID too large.',250,0,4,null,0
exec SP_MID_SET_TEXT 050075,'Application error. Color code is not on file.',250,0,6,null,0
exec SP_MID_SET_TEXT 050076,'Color already in style.',250,0,5,null,0
exec SP_MID_SET_TEXT 050077,'Color code does not meet defined requirements.',250,0,4,null,0
exec SP_MID_SET_TEXT 050078,'Size code does not meet defined requirements.',250,0,4,null,0
exec SP_MID_SET_TEXT 050079,'Color level not found.  Can not add color code.',250,0,6,null,0
exec SP_MID_SET_TEXT 050080,'Can not add colors to hardline styles/skus.',250,0,5,null,0
exec SP_MID_SET_TEXT 050081,'Week range overlaps.  Can not apply daily percentages to this store.',250,0,5,null,0
exec SP_MID_SET_TEXT 050082,'Unable to update {0} stores because of overlapping date ranges.',250,0,5,null,0
exec SP_MID_SET_TEXT 050083,'Velocity grades must be unique.',250,0,4,null,0
exec SP_MID_SET_TEXT 050084,'Store grades must be unique.',250,0,4,null,0
exec SP_MID_SET_TEXT 050085,'Grade boundaries must be unique.',250,0,4,null,0
exec SP_MID_SET_TEXT 050086,'The last boundary must be zero.',250,0,4,null,0
exec SP_MID_SET_TEXT 050087,'Total daily percentages must equal 100 before they can be applied',250,0,4,null,0
exec SP_MID_SET_TEXT 050088,'Allocation Minimum must be less than or equal to Allocation Maximum',250,0,4,null,0
exec SP_MID_SET_TEXT 050089,'Color Minimum must be less than or equal to Color Maximum',250,0,4,null,0
exec SP_MID_SET_TEXT 050090,'Do you want to apply to all descendants for this node?',250,0,4,null,0
exec SP_MID_SET_TEXT 050091,'The last sell thru percentage must be zero.',250,0,4,null,0
exec SP_MID_SET_TEXT 050092,'Must be renamed in its home location.',250,0,4,null,0
exec SP_MID_SET_TEXT 050093,'Date is required before daily percentages can be applied',250,0,4,null,0
exec SP_MID_SET_TEXT 050094,'Color minimum is greater than minimum stock.',250,0,4,null,0
exec SP_MID_SET_TEXT 050095,'Color maximum is greater than maximum stock.',250,0,4,null,0
exec SP_MID_SET_TEXT 050096,'Can not drop node here because it would cause a circular relationship.',250,0,5,null,0
exec SP_MID_SET_TEXT 050097,'Are you sure you want to delete shortcut {0}?',250,0,2,null,0
exec SP_MID_SET_TEXT 050098,'All stores have not been assigned a capacity value.  Do you want to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 050099,'Please enter a capacity value for each store',250,0,2,null,0
exec SP_MID_SET_TEXT 050100,'Inherited capacity values can not be deleted.  Do you want to replace the value with 0 to stop the inheritance?',250,0,2,null,0
exec SP_MID_SET_TEXT 050101,'{0} is not a parent of {1} in hierarchy {2}',250,0,5,null,0
exec SP_MID_SET_TEXT 050102,' {0} {1} not found.  Would you like to add it?',250,0,2,null,0
exec SP_MID_SET_TEXT 050103,'Plan View cannot be accessed from this folder.',250,0,6,null,0
exec SP_MID_SET_TEXT 050104,'Product is not on file.  Auto add is disabled.',250,0,5,null,0
exec SP_MID_SET_TEXT 050105,'Nodes can only be added in the home hierarchy of parent node.',250,0,5,null,0
exec SP_MID_SET_TEXT 050106,'Node already a member of this hierarchy',250,0,5,null,0
exec SP_MID_SET_TEXT 050107,'Size code was not added.  Node can not be added.',250,0,5,null,0
exec SP_MID_SET_TEXT 050108,'Do you want to add a level after {0}?',250,0,5,null,0
exec SP_MID_SET_TEXT 050109,'Do you want a Color level added to the hierarchy?',250,0,5,null,0
exec SP_MID_SET_TEXT 050110,'Do you want a Size level added to the hierarchy?',250,0,5,null,0
exec SP_MID_SET_TEXT 050111,'Can not move level because there are nodes defined at the next level in the hierarchy.',250,0,5,null,0
exec SP_MID_SET_TEXT 050112,'Size level not found.  Can not add size code.',250,0,6,null,0
exec SP_MID_SET_TEXT 050113,'You must first delete all lower levels.',250,0,5,null,0
exec SP_MID_SET_TEXT 050114,'Invalid OTS Forecast Level',250,0,4,null,0
exec SP_MID_SET_TEXT 050115,'A node can not be moved from a shared path',250,0,4,null,0
exec SP_MID_SET_TEXT 050116,'Are you sure you want to remove these {0} items',250,0,5,null,0
exec SP_MID_SET_TEXT 050117,'Are you sure you want to delete these {0} items',250,0,5,null,0
exec SP_MID_SET_TEXT 050118,'Are you sure you want to delete/remove these {0} items ',250,0,5,null,0
exec SP_MID_SET_TEXT 050119,'You are not authorized to delete all the selected nodes.  Do you want to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 050120,'You are not authorized to remove all the selected nodes.  Do you want to continue?',250,0,4,null,0
exec SP_MID_SET_TEXT 050121,'You are not authorized to delete/remove all the selected nodes.  Do you want to continue?',250,0,4,null,0
exec SP_MID_SET_TEXT 050122,'To Parent is required',250,0,4,null,0
exec SP_MID_SET_TEXT 050123,'New product ID is required',250,0,4,null,0
exec SP_MID_SET_TEXT 050124,'*** Reclass executed for preview purposes only ***',250,0,2,null,0
exec SP_MID_SET_TEXT 050125,'Move {0} from {1} to {2} in hierarchy {3}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050126,'{0} has been moved from {1} to {2} in hierarchy {3}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050127,'Rename {0} to {1} with name {2} and description {3}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050128,'{0} has been renamed to {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050129,'Delete {0} from parent {1} in hierarchy {2}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050130,'{0} has been deleted from parent {1} in hierarchy {2}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050131,'{0} has been removed from parent {1} in hierarchy {2}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050132,'{0} was not deleted from parent {1} in hierarchy {2}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050133,'{0} was not removed from parent {1} in hierarchy {2}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050134,'Rollup has been scheduled for {0} product {1} version {2} from {3} thru {4}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050135,'Delete product {0} from security group {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050136,'Delete product {0} from security for user {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050137,'Remove product {0} from {1} for user {2}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050138,'Replace product {0} with product {1} in {2} for user {3}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050139,'Product {0} can not be deleted because it is in use by header {1}.',250,0,5,null,0
exec SP_MID_SET_TEXT 050140,'*** Reclass failed.  Process continued for preview purposes only ***',250,0,2,null,0
exec SP_MID_SET_TEXT 050141,'{0} has not been moved from {1} to {2} in hierarchy {3}.',250,0,6,null,0
exec SP_MID_SET_TEXT 050142,'{0} has not been renamed to {1}.',250,0,6,null,0
exec SP_MID_SET_TEXT 050143,'Transaction type is invalid.',250,0,6,null,0
exec SP_MID_SET_TEXT 050144,'Options forecast version rollup flag is invalid. Forecast version will not be rolled.',250,0,6,null,0
exec SP_MID_SET_TEXT 050145,'Action is invalid.',250,0,6,null,0
exec SP_MID_SET_TEXT 050146,'Preview flag is invalid.  Preview automatically enabled.',250,0,6,null,0
exec SP_MID_SET_TEXT 050147,'Option record type is invalid.',250,0,6,null,0
exec SP_MID_SET_TEXT 050148,'Force delete flag is invalid.  Force delete automatically disabled.',250,0,6,null,0
exec SP_MID_SET_TEXT 050149,'Reclass preview is off',250,0,2,null,0
exec SP_MID_SET_TEXT 050150,'The following nodes could not be locked:',250,0,4,null,0
exec SP_MID_SET_TEXT 050151,'   Node: {0} is in use by {1}',250,0,4,null,0
exec SP_MID_SET_TEXT 050152,'All requested nodes are in use.  Delete cancelled.',250,0,4,null,0
exec SP_MID_SET_TEXT 050153,'All nodes could not be locked.  Do you wish to delete the nodes that were unlocked?',250,0,5,null,0
exec SP_MID_SET_TEXT 050154,'{0} will be deleted',250,0,4,null,0
exec SP_MID_SET_TEXT 050155,'{0} will not be deleted',250,0,4,null,0
exec SP_MID_SET_TEXT 050156,'Do you want to make a reference to the selected nodes under {0}?',250,0,5,null,0
exec SP_MID_SET_TEXT 050157,'Do you want to move the selected nodes to {0}?',250,0,5,null,0
exec SP_MID_SET_TEXT 050158,'Do you want to copy the selected nodes to {0}?',250,0,5,null,0
exec SP_MID_SET_TEXT 050159,'Delete of {0} is not allowed because style {1} is referenced on headers',250,0,6,null,0
exec SP_MID_SET_TEXT 050160,'Move of {0} is not allowed because style {1} is referenced on headers',250,0,6,null,0
exec SP_MID_SET_TEXT 050161,'Reclass is not permitted at the size level',250,0,6,null,0
exec SP_MID_SET_TEXT 050162,'A shortcut has been made to {0} in {1} in hierarchy {2}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050163,'The parent is not part of the hierarchy',250,0,6,null,0
exec SP_MID_SET_TEXT 050164,'{0} is already assigned the value {1}.  Do you want to replace with {2}?',250,0,5,null,0
exec SP_MID_SET_TEXT 050165,'Product characteristic not found',250,0,4,null,0
exec SP_MID_SET_TEXT 050166,'Product characteristic is required',250,0,4,null,0
exec SP_MID_SET_TEXT 050167,'Product characteristic {0} has been added',250,0,2,null,0
exec SP_MID_SET_TEXT 050168,'Product characteristic value not found',250,0,4,null,0
exec SP_MID_SET_TEXT 050169,'Product characteristic value {0} has been added',250,0,2,null,0
exec SP_MID_SET_TEXT 050170,'Invalid node for OTS hierarchy',250,0,4,null,0
exec SP_MID_SET_TEXT 050171,'Invalid OTS level select type',250,0,4,null,0
exec SP_MID_SET_TEXT 050172,'Invalid OTS node search type',250,0,4,null,0
exec SP_MID_SET_TEXT 050173,'OTS Level is required',250,0,4,null,0
exec SP_MID_SET_TEXT 050174,'OTS Forecast Starts With is required',250,0,4,null,0
exec SP_MID_SET_TEXT 050175,'Changing this value will cause the current stock min/max information to be immediately erased.  Do you wish to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 050176,'Product must be renamed from home parent',250,0,4,null,0
exec SP_MID_SET_TEXT 050177,'Product Category required to add size code',250,0,4,null,0
exec SP_MID_SET_TEXT 050178,'Unable to delete {0} because it is in use by {1}.',250,0,5,null,0
exec SP_MID_SET_TEXT 050179,'Roll External Intransit flag is invalid.  External Intransit will not be rolled.',250,0,6,null,0
exec SP_MID_SET_TEXT 050180,'Roll Alternate Hierarchies flag is invalid.  Alternate Hierarchies will not be rolled.',250,0,6,null,0
/* Begin TT#155 - JScott - Add Size Curve info to Node Properties */
exec SP_MID_SET_TEXT 050181,'Inherited Default Size Curves can not be unchecked.',250,0,6,null,0
/* Begin TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning. */
/* exec SP_MID_SET_TEXT 050182,'Curve Name not unique.',250,0,6,null,0 */
exec SP_MID_SET_TEXT 050182,'Duplicate size curve names have been requested.  Add a Curve Name to make unique.',250,0,6,null,0
/* End TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning. */
/* Begin TT#274 - RMatelic - Purge Criteria report issues */
exec SP_MID_SET_TEXT 050183,'** = Inherited from higher node or level',250,0,6,null,0
exec SP_MID_SET_TEXT 050184,'You must include at least one subreport',250,0,6,null,0 
/* End TT#274  */  
exec SP_MID_SET_TEXT 050185,'Only one size curve similar store allowed.',250,0,6,null,0
/* End TT#155 - JScott - Add Size Curve info to Node Properties */
/* Begin TT#483 - JScott - Add Size Lost Sales criteria and processing */
exec SP_MID_SET_TEXT 050186,'Parent node has a different Store Group defined.  Changing the Store Group will detach all inheritance for Out of Stock values for this node.\n Do you wish to continue?',250,0,5,null,0
/* End TT#483 - JScott - Add Size Lost Sales criteria and processing */
/* Begin TT#634 - JSmith - Color rename */
exec SP_MID_SET_TEXT 050187,'Color {0} has been renamed to {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050188,'Color {0} will replace color {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050189,'Color {0} has been added.',250,0,2,null,0
/* End TT#634 */
/* Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model */
exec SP_MID_SET_TEXT 050190,'Restarting Hierarchy Service',30,2,null,null,0
exec SP_MID_SET_TEXT 050191,'Hierarchy Service could not be stopped in a timely manner. Check Event Viewer.',30,7,null,null,0
exec SP_MID_SET_TEXT 050192,'Hierarchy Service could not be started in a timely manner. Check Event Viewer.',30,7,null,null,0
exec SP_MID_SET_TEXT 050193,'Service {0} not found.',30,7,null,null,0
exec SP_MID_SET_TEXT 050194,'Starting Hierarchy Service',30,2,null,null,0
exec SP_MID_SET_TEXT 050195,'Hierarchy Service successfully started',30,2,null,null,0
exec SP_MID_SET_TEXT 050196,'Stoping Hierarchy Service',30,2,null,null,0
exec SP_MID_SET_TEXT 050197,'Hierarchy Service successfully stopped',30,2,null,null,0
/* End TT#988 */
/*Begin TT#1323 - JSmith - Rollup never completed and had to be cancelled */
exec SP_MID_SET_TEXT 050198,'The following hierarchies contain circular relationships:',250,0,5,null,0
exec SP_MID_SET_TEXT 050199,'Hierarchy {0} references hierarchies {1}',250,0,5,null,0
/*End TT#1323 */
/*Begin TT#1159 – improving messaging*/
exec SP_MID_SET_TEXT 050200,' ({0}) {1} is in use by user {2}.',50,2,null,null,0
/*End TT#1159 – improving messaging*/
/*Begin TT#1399 - GTaylor - Alternate Hierarchy Inherit Node Properties */
exec SP_MID_SET_TEXT 050201,'From',10,0, null,null,0
exec SP_MID_SET_TEXT 050202,'Apply Node Properties',30,0, null,null,0
exec SP_MID_SET_TEXT 050203,'Must be alternate hierarchy node to set Apply Node Properties',30,0, null,null,0
exec SP_MID_SET_TEXT 050204,'Apply Node Properties node not found',30,0, null,null,0
exec SP_MID_SET_TEXT 050205,'Apply Node Properties can only be set to an organizational node',30,0, null,null,0
/*End TT#1399 - GTaylor - Alternate Hierarchy Inherit Node Properties */
/* Begin TT#1873 - JSmith - Purge Failed with Severe Error */
exec SP_MID_SET_TEXT 050206,'All items for user {0} were not deleted.  User will not be deleted.',250,0,5,null,0
exec SP_MID_SET_TEXT 050207,'Nodes in hierarchy {0} are referenced by headers.  Hierarchy will not be deleted.',250,0,5,null,0
/* End TT#1873 - JSmith - Purge Failed with Severe Error */

/*Begin TT#1754 - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy */
exec SP_MID_SET_TEXT 050208,'Only one organizational hierarchy is allowed',250,0,6,null,0
/*End TT#1754 - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy */
/* Begin TT#2621 - JSmith - Duplicate weeks in daily sales */
exec SP_MID_SET_TEXT 050209,'Duplicate date range of {0}.  Please select different date range or adjust existing date range.',250,0,4,null,0
/* End TT#2621 - JSmith - Duplicate weeks in daily sales */
exec SP_MID_SET_TEXT 050210,'User Alternate Node cannot be referenced in a Global or other User''s Alternate',250,0,4,null,0  -- TT#2737 - JSmith - Brand Hierarchy Reclass
/* End Hierarchy Messages 050000-059999 */
exec SP_MID_SET_TEXT 050211,'To parent not found',250,0,4,null,0   -- TT#3366 - JSmith - Parent not found error in Audit is deceiving
/* Begin TT#3894-VStuart-Alternate Reclass Failure-Oakley */
exec SP_MID_SET_TEXT 050212,'Invalid trigger file. Does not have a corresponding transaction file.',110,0,5,null,0
/* End TT#3894-VStuart-Alternate Reclass Failure-Oakley */

/* Begin TT#1646-MD - jsmith - Add or Remove Hierarchy Levels */
exec SP_MID_SET_TEXT 050213, 'Invalid level name.',250,0,6,null,0
exec SP_MID_SET_TEXT 050214, 'Cannot add level after style.',250,0,6,null,0
exec SP_MID_SET_TEXT 050215, 'Duplicate level name.',250,0,6,null,0
exec SP_MID_SET_TEXT 050216, 'Incorrect number of fields to add a level.',250,0,6,null,0
exec SP_MID_SET_TEXT 050217, 'Level {0} was successfully added to hierarchy {1} after level {2}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050218, 'Level {0} was successfully removed from hierarchy {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 050219, 'Hierarchy {0} has {1} home levels.',250,0,2,null,0
/* End TT#1646-MD - jsmith - Add or Remove Hierarchy Levels */

/* TT#5245-Purge error number 2-BonTon */
exec SP_MID_SET_TEXT 050220,'User {0} has nodes in a hierarchy that are In Use by the following items. User will not be deleted.',250,0,5,null,0
exec SP_MID_SET_TEXT 050221,'NODE: {0};  TYPE: {1};  NAME: {2}  OWNER: {3}.',250,0,5,null,0
exec SP_MID_SET_TEXT 050222,'Hierarchy {0} is not set for API Rollup.  Actuals will not be processed.',250,0,5,null,0  /* TT#5479 - JSmith - Rollup should not build requests for hierarchies not set as API  */
exec SP_MID_SET_TEXT 050223,'Merchandise cannot be from an alternate hierarchy.',250,0,2,null,0  /* TT#2108-MD - JSmith - An Alternate Hierachy can be used in the Apply to for Merchandise.  This is not allowed because when charging intransit it would not know where to put it.  */

/* End Hierarchy Messages 050000-059999 */

/* Store Messages 060000-069999 */
exec SP_MID_SET_TEXT 060000,'Store is required',250,0,5,null,0
exec SP_MID_SET_TEXT 060001,'Store not found',250,0,5,null,0
exec SP_MID_SET_TEXT 060002,'Active Indicator not provided.  Using default of Active.',250,0,5,null,0
exec SP_MID_SET_TEXT 060003,'Attribute Set Name is required',250,0,5,null,0
exec SP_MID_SET_TEXT 060004,'Error adding/updating text in where clause.',250,0,6,null,0
exec SP_MID_SET_TEXT 060005,'Error removing text from where clause.',250,0,6,null,0
exec SP_MID_SET_TEXT 060006,'Error while switching between radio buttons',250,0,6,null,0
exec SP_MID_SET_TEXT 060007,'Error populating all stores list box.',250,0,6,null,0
exec SP_MID_SET_TEXT 060008,'Error populating store characteristics list box.',250,0,6,null,0
exec SP_MID_SET_TEXT 060009,'Error populating where clause.',250,0,6,null,0
exec SP_MID_SET_TEXT 060010,'Error saving Attribute Set definition.',250,0,6,null,0
exec SP_MID_SET_TEXT 060011,'Error adding all items to listbox.',250,0,6,null,0
exec SP_MID_SET_TEXT 060012,'Error removing all items from listbox.',250,0,6,null,0
exec SP_MID_SET_TEXT 060013,'Error adding item to listbox.',250,0,6,null,0
exec SP_MID_SET_TEXT 060014,'Error determining Store Characteristic.',250,0,6,null,0
exec SP_MID_SET_TEXT 060015,'Error determining boolean Store Characteristic.',250,0,6,null,0
exec SP_MID_SET_TEXT 060016,'Error determining string Store Characteristic.',250,0,6,null,0
exec SP_MID_SET_TEXT 060017,'Error determining numeric Store Characteristic.',250,0,6,null,0
exec SP_MID_SET_TEXT 060018,'Error determining date Store Characteristic.',250,0,6,null,0
exec SP_MID_SET_TEXT 060019,'Error determining user defined Store Characteristic.',250,0,6,null,0
exec SP_MID_SET_TEXT 060020,'Error clearing values.',250,0,6,null,0
exec SP_MID_SET_TEXT 060021,'Error filling date values.',250,0,6,null,0
exec SP_MID_SET_TEXT 060022,'From Date must be earlier than to date.',250,0,5,null,0
exec SP_MID_SET_TEXT 060023,'Error validating date values.',250,0,6,null,0
exec SP_MID_SET_TEXT 060024,'An operator must be selected before adding statement.',250,0,5,null,0
exec SP_MID_SET_TEXT 060025,'Error:  Null value in Database.  Store_Group_Level_Statement must be built by Attribute Set Builder.',250,0,6,null,0
exec SP_MID_SET_TEXT 060026,'Selling Open Date is greater than Selling Close Date.',250,0,5,null,0
exec SP_MID_SET_TEXT 060027,'Stock Open Date is greater than Stock Close Date.',250,0,5,null,0
exec SP_MID_SET_TEXT 060028,'Store Characteristic Value not defined in Characteristic list.',250,0,6,null,0
exec SP_MID_SET_TEXT 060029,'Store Characteristic not found.',250,0,6,null,0
exec SP_MID_SET_TEXT 060030,'Invalid between values found for attribute set criteria.',250,0,6,null,0
exec SP_MID_SET_TEXT 060031,'One or more Attributes/Sets are using the characteristic: {0}. It cannot be removed. See the list below for where it is being used.',250,0,5,null,0
exec SP_MID_SET_TEXT 060032,'Please complete the Characteristic Group before adding values to it.',250,0,4,null,0
exec SP_MID_SET_TEXT 060033,'Store Status is a System Characteristic and cannot be changed.',250,0,5,null,0
exec SP_MID_SET_TEXT 060034,'Stores already contain values for this characteristic.  If you continue, these values will be added to the value list as well.',250,0,5,null,0
exec SP_MID_SET_TEXT 060035,'The value, {0}, that you wish to delete is already used by some stores.  If you continue, these values will be removed from the stores. Continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 060036,'Duplicate Characteristic Name or Store Field defined.',250,0,5,null,0
exec SP_MID_SET_TEXT 060037,'Duplicate Characteristic Value defined.',250,0,5,null,0
exec SP_MID_SET_TEXT 060038,'Characteristic Name Conflict. The Characteristic name you have chosen conflicts with the name of a current Store Attribute. Characteristics, once saved, become Dynamic Store Attributes. Please change the name of the Characteristic.',250,0,5,null,0
exec SP_MID_SET_TEXT 060039,'This Characteristic only accepts Numeric Values.',250,0,4,null,0
exec SP_MID_SET_TEXT 060040,'This Characteristic only accepts Date Values.',250,0,4,null,0
exec SP_MID_SET_TEXT 060041,'The dollar value is missing or invalid.',250,0,4,null,0
exec SP_MID_SET_TEXT 060042,'One or more dollar values are invalid.',250,0,4,null,0
exec SP_MID_SET_TEXT 060043,'One or more numeric values are missing or invalid.',250,0,4,null,0
exec SP_MID_SET_TEXT 060044,'The numeric value is missing or invalid.',250,0,4,null,0
exec SP_MID_SET_TEXT 060045,'One or more date values are missing or invalid.',250,0,4,null,0
exec SP_MID_SET_TEXT 060046,'The date value is missing or invalid.',250,0,4,null,0
exec SP_MID_SET_TEXT 060047,'The boolean value is missing or invalid.',250,0,4,null,0
exec SP_MID_SET_TEXT 060048,'The text value is missing.',250,0,4,null,0
exec SP_MID_SET_TEXT 060049,'Headers already contain values for these characteristics. If you continue, these values will be added to the value list as well. Continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 060050,'The values you wish to delete are already in use by some headers. If you continue, these values will be removed from the headers. This change can not be canceled once you click OK. Continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 060051,'The characteristic groups you wish to delete have sub values that are in use by some headers. If you continue, these values will be removed from the header. This change can not be canceled once you click OK. Continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 060052,'While trying to update the underlying data table an error occurred. So the update failed.',250,0,4,null,0
exec SP_MID_SET_TEXT 060053,'While trying to get back one record from the data table multiple records were received this was not accepted.',250,0,4,null,0
exec SP_MID_SET_TEXT 060054,'You are unable to exit while you are editing a cell.',250,0,4,null,0
exec SP_MID_SET_TEXT 060055,'The characteristic, {0}, that you wish to delete is already in use by some stores. If you continue, these values will be removed from the stores. Continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 060056,'The characteristics are not all paired with values. Most commonly this is caused by an extra delimiter at end of the data line.',250,0,4,null,0
exec SP_MID_SET_TEXT 060057,'There are mismatched Parentheses in the ''Select Stores Where'' text box.',250,0,5,null,0
exec SP_MID_SET_TEXT 060058,'Invalid SQL Construct:  {0}  Changes have not been saved. Please report this to Technical Support.',250,0,6,null,0
exec SP_MID_SET_TEXT 060059,'Are you sure you want to remove {0}? Remember to do a system refresh after completing all of your deletes.',250,0,2,null,0
exec SP_MID_SET_TEXT 060060, 'One or more workflows are using the method: {0}. It cannot be removed. See the list below for where it is being used.',250,0,6,null,0
exec SP_MID_SET_TEXT 060061, 'One or more methods are using the attribute: {0}. It cannot be removed. See the list below for where it is being used.',250,0,6,null,0
/*Begin Track #4010 - JScott - Add all forecast methods and workflows to the scheduler. */
exec SP_MID_SET_TEXT 060062, 'One or more Task Lists are using the workflow: {0}. It cannot be removed. See the list below for where it is being used.',250,0,6,null,0
/*End Track #4010 - JScott - Add all forecast methods and workflows to the scheduler. */
exec SP_MID_SET_TEXT 060063, 'The Characteristic Group name you have chosen ({0}) matches a reserved word and cannot be used as a characteristic name. Please change the characteristic name.',250,0,6,null,0
exec SP_MID_SET_TEXT 060064, 'Store {0} is not active',250,0,5,null,0 /* MID Track 4214 Identify stores in messages */
exec SP_MID_SET_TEXT 060065, 'Value must be positive.',250,0,5,null,0
exec SP_MID_SET_TEXT 060066, 'Store active flag has been changed.  Changing this flag while other windows are open or other users are logged in may affect the results.  Do you want to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 060067, 'Invalid Store Field',250,0,4,null,0
exec SP_MID_SET_TEXT 060068, 'Store: {0}  Field: {1}  Value: {2}.',250,0,4,null,0
exec SP_MID_SET_TEXT 060069, 'Unable to delete: {0}.',250,0,5,null,0
exec SP_MID_SET_TEXT 060070, 'Store {0}. Transaction does not contain the minimum number of expected fields. This typically indicates one or more missing delimiters or that the delimiter defined in the config file ({1}) does not match the input file delimiter.',250,0,5,null,0
exec SP_MID_SET_TEXT 060071, 'Characteric name: {0}  matches existing header column or header characteristic name. Please choose a different name.',250,0,5,null,0
exec SP_MID_SET_TEXT 060072, 'The Store Group {0} is in use by User {1}.',250,0,5,null,0
/* Begin TT#166 stodd */
exec SP_MID_SET_TEXT 060073, 'In OPTIONS, Characteristic Auto Add value is invalid. Value is {0}.',250,0,4,null,0
exec SP_MID_SET_TEXT 060074, 'In OPTIONS, Characteristic Type Delimiter can only be a single character. Delimiter value is {0}.',250,0,4,null,0
exec SP_MID_SET_TEXT 060075, 'Invalid Store Characteristic Type for auto-add characteristic, {0}. Char type = {1}',250,0,4,null,0
exec SP_MID_SET_TEXT 060076, 'Store Characteristic, {0}, was auto added.',250,0,3,null,0
/* End TT#166 stodd */
/* Begin TT#237 - JSmith - after transaction gets error, subsequent transactions not processed. */
exec SP_MID_SET_TEXT 060077, 'Store Characteristic value does not match type.',250,0,4,null,0
/* End TT#237 */
exec SP_MID_SET_TEXT 060078, 'Store Characteristic has duplicate attribute.',250,0,4,null,0
exec SP_MID_SET_TEXT 060079, 'Store Characteristics cannot be added prior to store being added.',250,0,4,null,0
exec SP_MID_SET_TEXT 060080, 'Store Service looped resolving leftover (available) stores in a dynamic store attribute. Attr (key)= {0} ({1}). Set (key)= {2} ({3}). Store (Key)= {4} ({5})',250,0,5,null,0
exec SP_MID_SET_TEXT 060081, 'Store must be marked as inactive to delete. Store: {0}.',250,0,4,null,0	/* TT#739-MD - stodd - delete stores - */
exec SP_MID_SET_TEXT 060082, 'The Reserve Store can not be deleted.',250,0,4,null,0	/* TT#739-MD - stodd - delete stores - */
exec SP_MID_SET_TEXT 060083, 'The Reserve Store can not be marked as inactive.',250,0,4,null,0	/* TT#739-MD - stodd - delete stores - */
exec SP_MID_SET_TEXT 060084, 'The system indicates that a Store Delete is already running. Only one Store Delete process may be run at a time.',250,0,6,null,0	/* TT#739-MD - stodd - delete stores - */
exec SP_MID_SET_TEXT 060085, 'No stores are marked for deletion.',250,0,6,null,0
exec SP_MID_SET_TEXT 060086, 'Logility RO Services must be down for for this process. Please shut down services and restart the process.',250,0,6,null,0
exec SP_MID_SET_TEXT 060087, 'The system indicates that the Delete Store Analysis was run over 7 days ago. Please rerun analysis or change the ''SkipAnalysis'' setting to false.',250,0,6,null,0
exec SP_MID_SET_TEXT 060088, 'If percentage of rows to delete are <= this value, results will be to delete table.',250,0,6,null,0
exec SP_MID_SET_TEXT 060089, 'If the rows to be removed are >= than this value, results will be to force copying the table.',250,0,6,null,0
exec SP_MID_SET_TEXT 060090, 'If rows to delete are <= this value, results will be to delete the rows from the table.',250,0,6,null,0
exec SP_MID_SET_TEXT 060091, 'It identifies the number of records within each batch during a delete process.',250,0,6,null,0
exec SP_MID_SET_TEXT 060092, 'It identifies the number of concurrent Delete from tables that are processed in tandem.',250,0,6,null,0
exec SP_MID_SET_TEXT 060093, 'It identifies the number of concurrent Copy/Load tables that are processed in tandem.',250,0,6,null,0
exec SP_MID_SET_TEXT 060094, 'Constraints on the database have not been dropped.',250,0,6,null,0
exec SP_MID_SET_TEXT 060095, 'Store Delete In Progress indicator has been reset.',250,0,6,null,0
exec SP_MID_SET_TEXT 060096, 'Saving settings forces a rerun of Analysis.',250,0,6,null,0
/* TT#858-MD - DOConnell – Do not allow a store to be set to Inactive if it has allocation quantities or Intransit - */
exec SP_MID_SET_TEXT 060097, 'Store has existing Allocation or Intransit units and cannot be set Inactive',250,0,6,null,0
/* Begin TT#5189 - JSmith – Audit Message - references stores with allocation units when not units are allocated to the stores - */
exec SP_MID_SET_TEXT 060098, 'Store has existing Allocation units or is the last store referencing the VSW ID. The VSW ID cannot be removed.',250,0,6,null,0  /* TT#4685 - JSmith – clear NODE_IMO table when VSW ID is removed from store */
/* End TT#5189 - JSmith – Audit Message - references stores with allocation units when not units are allocated to the stores - */
exec SP_MID_SET_TEXT 060099, 'There are pending changes.  Do you wish to exit?',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060100, 'Store ID is a required field.',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060101, 'The Reserve Store''s ID cannot be changed.',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060102, 'Invalid Date Format.',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060103, 'Date is not within the range of the merchandise calendar.',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060104, 'Selling Close Date is prior to Selling Open Date.',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060105, 'Stock Close Date is prior to Stock Open Date.',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060106, 'Store ID must be unique.  Enter a unique Store ID before continuing.',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060107, 'Selling Sq. Ft. must be a positive number.',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060108, 'Lead Time must be a positive number.',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060109, 'Cell value not valid.',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060110, 'Store ID can not exceed 50 characters.',250,0,4,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 060111, 'An Attribute with the name you specified already exists.  Please specify a different name.',250,0,6,null,0  /* TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed. */
exec SP_MID_SET_TEXT 060112, 'Attribute Name Already Exists',250,0,6,null,0  /* TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed. */
exec SP_MID_SET_TEXT 060113, 'Stores or attributes have been added or removed.  You must do a Tools>Refresh before continuing.',250,0,5,null,0    /* TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail  */

/* Variable Messages 070000-074999 */
exec SP_MID_SET_TEXT 070000,'Chain daily forecast not valid',250,0,6,null,0
exec SP_MID_SET_TEXT 070001,'Store daily forecast not valid',250,0,6,null,0
exec SP_MID_SET_TEXT 070002,'Variable name is required',250,0,4,null,0
exec SP_MID_SET_TEXT 070003,'Variable name not found',250,0,4,null,0
exec SP_MID_SET_TEXT 070004,'Variable name not saved to database',250,0,4,null,0
exec SP_MID_SET_TEXT 070005,'Intransit must be by day',250,0,4,null,0
exec SP_MID_SET_TEXT 070006,'Invalid attribute for element {0}: {1}',250,0,4,null,0
exec SP_MID_SET_TEXT 070007,'History can not be loaded to total.',250,0,4,null,0
exec SP_MID_SET_TEXT 070008,'Intransit can not be prior to the posting date.',250,0,4,null,0
exec SP_MID_SET_TEXT 070009,'At least one variable must be selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 070010,'Variable {0} is not a database variable and can not be rolled',250,0,4,null,0 
exec SP_MID_SET_TEXT 070011,'Invalid roll level {0} for variable {1}.  The variable will not roll for this type.',250,0,4,null,0 
exec SP_MID_SET_TEXT 070012,'Variable roll level definitions not found in input file.  Roll not processed.',250,0,4,null,0
exec SP_MID_SET_TEXT 070013,'Value not valid for type of data.',250,0,4,null,0

/* Plan Method Messages 075000-079999 */
exec SP_MID_SET_TEXT 075000,'Merchandise Level already exists as default.',250,0,5,0,0
exec SP_MID_SET_TEXT 075001,'Error loading OTS Forecast Method.',250,0,5,0,0
exec SP_MID_SET_TEXT 075002,'Error executing forecast method',250,0,6,0,0
exec SP_MID_SET_TEXT 075003,'Week not forecasted. Week is prior to Current week.',250,0,6,0,0
exec SP_MID_SET_TEXT 075004,'Cannot insert Method.  Method Name, Type, and User must be unique.',250,0,6,0,0
exec SP_MID_SET_TEXT 075005,'Cannot insert Method.  Undefined Error.',250,0,6,0,0
exec SP_MID_SET_TEXT 075006,'MethodReserved2',250,0,6,0,0
exec SP_MID_SET_TEXT 075007,'MethodReserved3',250,0,6,0,0
exec SP_MID_SET_TEXT 075008,'MethodReserved4',250,0,6,0,0
exec SP_MID_SET_TEXT 075009,'MethodReserved5',250,0,6,0,0
exec SP_MID_SET_TEXT 075010,'MethodReserved6',250,0,6,0,0
exec SP_MID_SET_TEXT 075011,'MethodReserved7',250,0,6,0,0
exec SP_MID_SET_TEXT 075012,'MethodReserved8',250,0,6,0,0
exec SP_MID_SET_TEXT 075013,'MethodReserved9',250,0,6,0,0
exec SP_MID_SET_TEXT 075014,'MethodReserved10',250,0,6,0,0
exec SP_MID_SET_TEXT 075015,'MethodReserved11',250,0,6,0,0
exec SP_MID_SET_TEXT 075016,'Store inventory cell value is protected for {0}, {1}, (2} and will not be updated.',250,0,5,0,0
exec SP_MID_SET_TEXT 075017,'There were no available cells to spread the chain value for {0} to the stores in week {1}.  The process was terminated.',250,0,5,0,0
exec SP_MID_SET_TEXT 075018,'There were no available cells to spread the value for {0} to the low levels for store {1} in week {2}.  The process was terminated.',250,0,5,0,0
exec SP_MID_SET_TEXT 075019,'There were no available cells to spread the chain value for {0} to the stores in period {1}.  The process was terminated.',250,0,5,0,0
exec SP_MID_SET_TEXT 075020,'There were no available cells to spread the value for {0} to the low levels for store {1} in period {2}.  The process was terminated.',250,0,5,0,0
exec SP_MID_SET_TEXT 075021,'Must be an OTS Forecast method to drop.',250,0,6,null,0
exec SP_MID_SET_TEXT 075022,'There were no available cells to spread {0} chain value to lower level for week {1}.',250,0,5,null,0
exec SP_MID_SET_TEXT 075023,'Chain total is 0. Could not calculate target weeks of supply for week {0}.',250,0,5,null,0
exec SP_MID_SET_TEXT 075024,'Target Weeks of Supply has exceeded the normal boundary. WOS = {0} for inventory week: {1}. WOS will be set to {2} weeks.',250,0,5,null,0
exec SP_MID_SET_TEXT 075025,'Chain total is 0. No Spread will be done for week {0}.',250,0,5,null,0
exec SP_MID_SET_TEXT 075026,'For week {0} WOS of {1} is beyond the normal boundary. If you continue, WOS will be set to 53 weeks for each week where WOS exceeds the boundary. Do you wish to continue forecasting?',250,0,5,null,0
exec SP_MID_SET_TEXT 075027,'Forecast was cancelled because week {0} Weeks of Supply of {1} weeks was beyond the normal boundary.',250,0,5,null,0
exec SP_MID_SET_TEXT 075028,'Inventory variable: {1} for Week: {0} not {2}. Forecast Version is protected.',250,0,5,null,0

/* BEGIN MID Track #4386 - Justin Bolles - Delete Views */
exec SP_MID_SET_TEXT 075029,'Global Views',250,0,5,null,0
exec SP_MID_SET_TEXT 075030,'User Views',250,0,5,null,0
/* END MID Track #4386 */

exec SP_MID_SET_TEXT 075031,'Trend Cap Tolerance not within 70% to 100% limits.',250,0,5,null,0
/* Begin Enhancement - JScott - Export Method - Part 2 */
exec SP_MID_SET_TEXT 075032,'Merchandise required',250,0,5,null,0
exec SP_MID_SET_TEXT 075033,'Version required',250,0,5,null,0
exec SP_MID_SET_TEXT 075034,'Time Period required',250,0,5,null,0
exec SP_MID_SET_TEXT 075035,'Delimeter required',250,0,5,null,0
exec SP_MID_SET_TEXT 075036,'File Path required',250,0,5,null,0
exec SP_MID_SET_TEXT 075037,'Number of Entries required',250,0,5,null,0
exec SP_MID_SET_TEXT 075038,'Flag Suffix required',250,0,5,null,0
exec SP_MID_SET_TEXT 075039,'End Suffix required',250,0,5,null,0
exec SP_MID_SET_TEXT 075040,'Concurrent Processes required',250,0,5,null,0
exec SP_MID_SET_TEXT 075041,'Suffixes must be different',250,0,5,null,0
exec SP_MID_SET_TEXT 075042,'Default File Format Invalid',250,0,5,null,0
exec SP_MID_SET_TEXT 075043,'Default Delimeter Invalid',250,0,5,null,0
exec SP_MID_SET_TEXT 075044,'Default File Path Invalid',250,0,5,null,0
exec SP_MID_SET_TEXT 075045,'Default Split Type Invalid',250,0,5,null,0
exec SP_MID_SET_TEXT 075046,'Default Split Number of Entries Invalid',250,0,5,null,0
exec SP_MID_SET_TEXT 075047,'Default Concurrent Processes Invalid',250,0,5,null,0
exec SP_MID_SET_TEXT 075048,'Default Flag File Suffix Invalid',250,0,5,null,0
exec SP_MID_SET_TEXT 075049,'Default End File Suffix Invalid',250,0,5,null,0
/* End Enhancement - JScott - Export Method - Part 2 */
exec SP_MID_SET_TEXT 075050,'Presentation MIn + Sales selected, yet no variable defined with a Client Custom Variable Type of Presentation Minimum.',250,0,6,null,0
/* Begin Track #4942 - JScott - Correct problems in Export Method */
exec SP_MID_SET_TEXT 075051,'Default Date Type Invalid',30,10,null,null,50
/* End Track #4942 - JScott - Correct problems in Export Method */
/* BEGIN Issue 5141 */
exec SP_MID_SET_TEXT 075052,'For week {0} and variable, {1}, all plan values are zeros and unlocked--no balancing needed.',250,0,5,null,0
/* END Issue 5141 */
exec SP_MID_SET_TEXT 075053,'Unable to delete store locks for {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 075054,'Unable to delete chain locks for {0}.',250,0,6,null,0
/* BEGIN 5401 addl Exception logic changes */
exec SP_MID_SET_TEXT 075055,'Method {0} Exception: {1}.',250,0,6,null,0
exec SP_MID_SET_TEXT 075056,'Method {0}, Set {1} is missing basis information for {2} ',250,0,6,null,0
exec SP_MID_SET_TEXT 075057,'Method {0}, Set {1} is missing trend cap information. Trend caps will be set to None.',250,0,6,null,0
/* END 5401 addl Exception logic changes */
/* Begin Track #5744 - JSmith - Rollup Method */
exec SP_MID_SET_TEXT 075058,'This will cause items in the base item table below to be deleted.  Are you sure you want to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 075059,'Do you want to keep the same entries in the base item table below?',250,0,2,null,0
/* End Track #5744 */
/* Begin Track #6187 - stodd */
exec SP_MID_SET_TEXT 075061,'Method {0} {1} {2} Warning: {3}.',250,0,5,null,0
exec SP_MID_SET_TEXT 075062,'Method: {0}, Variable: {1}. Locked store total({2}) > chain total({3}).  No Spread will be done for week {4}.',250,0,6,null,0

/* End Track #6187 */
exec SP_MID_SET_TEXT 075063,'Chain total is negative. No Spread will be done for week {0}.',250,0,5,null,0

/* Begin TT#1332 - JSmith - Object Reference Error when changing data in an existing OTS method */
exec SP_MID_SET_TEXT 075064,'Changing the current Merchandise will remove all settings for low level nodes.  Do you want to continue?',250,0,5,null,0
/* End TT#1332 */

exec SP_MID_SET_TEXT 075065,'Duplicate descendant {0} found. Method cannot be processed.', 250,0,5,null,0
exec SP_MID_SET_TEXT 075066,'Unable to set store locks for {0}.',250,0,6,null,0 /* TT#43 – MD – DOConnell – Projected Sales Enhancement */
exec SP_MID_SET_TEXT 075067,'Unable to set chain locks for {0}.',250,0,6,null,0 /* TT#43 – MD – DOConnell – Projected Sales Enhancement */
exec SP_MID_SET_TEXT 075068,'For method, {0}, no weeks to plan. All weeks are prior to current week.',250,0,6,null,0 /* TT#3166 – stodd – error running forecast */
exec SP_MID_SET_TEXT 075069,'You are not authorized to view all data referenced by this method. \nUpdate has been overridden to Read-Only.',250,0,5,null,0 /* TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error. */

/* Allocation Messages 080000-084999 */
exec SP_MID_SET_TEXT 080000,'Maximum is less than requested total',250,0,5,null,0
exec SP_MID_SET_TEXT 080001,'Minimums were broken',250,0,5,null,0
exec SP_MID_SET_TEXT 080002,'Spread total is not equal to requested total',250,0,5,null,0
exec SP_MID_SET_TEXT 080003,'No Eligible Items',250,0,5,null,0
exec SP_MID_SET_TEXT 080004,'Store Grade Definitions are Incomplete',250,0,5,null,0
exec SP_MID_SET_TEXT 080005,'No Period has Positive Sales or Beginning Stock',250,0,5,null,0
exec SP_MID_SET_TEXT 080006,'Average Beginning Stock is Zero',250,0,5,null,0
exec SP_MID_SET_TEXT 080007,'Quantity Allocated cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080008,'Maximum cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080009,'Minimum Allocated cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080010,'Primary Maximum cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080011,'Auto Allocation cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080012,'Rule Allocation cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080013,'Quantity Shipped cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080014,'Cannot Retrieve Ship Day when Ship Date Not Created',250,0,6,null,0
exec SP_MID_SET_TEXT 080015,'Cannot Retrieve Ship Week when Ship Date Not Created',250,0,6,null,0
exec SP_MID_SET_TEXT 080016,'Cannot Retrieve Need Day when Last Need Date Not Created',250,0,6,null,0
exec SP_MID_SET_TEXT 080017,'Cannot Retrieve Need Week when Last Need Date Not Created',250,0,6,null,0
exec SP_MID_SET_TEXT 080018,'Accumulated Generic Allocation cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080019,'Accumulated Bulk Color Allocation cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080020,'Ship Date must be created before Last Need Date can be created',250,0,6,null,0
exec SP_MID_SET_TEXT 080021,'Get request out of range for a byte flag',250,0,7,null,0
exec SP_MID_SET_TEXT 080022,'Set request out of range for a byte flag',250,0,7,null,0
exec SP_MID_SET_TEXT 080023,'Get request out of range for a short flag',250,0,7,null,0
exec SP_MID_SET_TEXT 080024,'Set request out of range for a short flag',250,0,7,null,0
exec SP_MID_SET_TEXT 080025,'Get request out of range for an integer flag',250,0,7,null,0
exec SP_MID_SET_TEXT 080026,'Set request out of range for an integer flag',250,0,7,null,0
exec SP_MID_SET_TEXT 080027,'Get request out of range for a long flag',250,0,7,null,0
exec SP_MID_SET_TEXT 080028,'Set request out of range for a long flag',250,0,7,null,0
exec SP_MID_SET_TEXT 080029,'Quantity to allocate cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080030,'Accumulated Bulk Size Allocation cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080031,'Multiple cannot be less than 1',250,0,6,null,0
exec SP_MID_SET_TEXT 080032,'Units in pack cannot be less than 1',250,0,6,null,0
exec SP_MID_SET_TEXT 080033,'Size not defined in pack color',250,0,6,null,0
exec SP_MID_SET_TEXT 080034,'Color not defined in pack',250,0,6,null,0
exec SP_MID_SET_TEXT 080035,'Size not defined for bulk color',250,0,6,null,0
exec SP_MID_SET_TEXT 080036,'Color not defined for bulk',250,0,6,null,0
exec SP_MID_SET_TEXT 080037,'Pack not defined on header',250,0,6,null,0
exec SP_MID_SET_TEXT 080038,'Pack size key in pack does not match expected size key',250,0,7,null,0
exec SP_MID_SET_TEXT 080039,'Number of stores must be greater than 0',250,0,6,null,0
exec SP_MID_SET_TEXT 080040,'Accumulated color units in pack cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080041,'Accumulated size units in pack color cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080042,'Pack color key in pack does not match expected color key',250,0,7,null,0
exec SP_MID_SET_TEXT 080043,'Accumulated bulk size units to allocate cannot be less than 0',250,0,6,null,0
exec SP_MID_SET_TEXT 080044,'Accumulated bulk color units to allocate cannot be less than 0',250,0,6,null,0
exec SP_MID_SET_TEXT 080045,'Bulk size key does not match expected size key',250,0,6,null,0
exec SP_MID_SET_TEXT 080046,'Bulk color key does not match expected color key',250,0,6,null,0
exec SP_MID_SET_TEXT 080047,'Pack name does not match expected pack name',250,0,6,null,0
exec SP_MID_SET_TEXT 080048,'Accumulated Generic units to allocate cannot be less than 0',250,0,6,null,0
exec SP_MID_SET_TEXT 080049,'Accumulated Detail units to allocate cannot be less than 0',250,0,6,null,0
exec SP_MID_SET_TEXT 080050,'Header is already a primary allocation must remove secondary header to reassign',250,0,6,null,0
exec SP_MID_SET_TEXT 080051,'Header is already a secondary allocation must remove primary header to reassign',250,0,6,null,0
exec SP_MID_SET_TEXT 080052,'Plan cannot be negative',250,0,6,null,0
exec SP_MID_SET_TEXT 080053,'Dimension must be greater than 0',250,0,7,null,0
exec SP_MID_SET_TEXT 080054,'Index Out of Range',250,0,7,null,0
exec SP_MID_SET_TEXT 080055,'Intransit cannot be negative',250,0,6,null,0
exec SP_MID_SET_TEXT 080056,'Grade week count must be greater than 0',250,0,6,null,0
exec SP_MID_SET_TEXT 080057,'Work up bulk buy and work up pack buy mutually exclusive',250,0,6,null,0
exec SP_MID_SET_TEXT 080058,'Reserve quantity cannot be negative',250,0,6,null,0
exec SP_MID_SET_TEXT 080059,'Capacity percent cannot be negative',250,0,6,null,0
exec SP_MID_SET_TEXT 080060,'Grade boundary cannot be negative',250,0,6,null,0
exec SP_MID_SET_TEXT 080061,'There were no eligible stores to calculate grade',250,0,6,null,0
exec SP_MID_SET_TEXT 080062,'Grade definitions missing or incomplete',250,0,6,null,0
exec SP_MID_SET_TEXT 080063,'Color not defined in Method',250,0,6,null,0
exec SP_MID_SET_TEXT 080064,'Store Set not defined in Method',250,0,6,null,0
exec SP_MID_SET_TEXT 080065,'Grade already exists duplicates are not allowed',250,0,6,null,0
exec SP_MID_SET_TEXT 080066,'Color already exists duplicates are not allowed',250,0,6,null,0
exec SP_MID_SET_TEXT 080067,'Store Set already exists duplicates not allowed',250,0,6,null,0
exec SP_MID_SET_TEXT 080068,'Pack already exists duplicates are not allowed',250,0,6,null,0
exec SP_MID_SET_TEXT 080069,'Grade is not defined in method',250,0,6,null,0
exec SP_MID_SET_TEXT 080070,'Tolerance percent cannot be negative',250,0,6,null,0
exec SP_MID_SET_TEXT 080071,'Unknown method in workflow',250,0,6,null,0
exec SP_MID_SET_TEXT 080072,'Invalid allocation action action cannot have user input',250,0,6,null,0
exec SP_MID_SET_TEXT 080073,'Plan percent factor cannot be less than zero',250,0,6,null,0
exec SP_MID_SET_TEXT 080074,'Allocation already started for header [{0}]. {1} [{2}] ignored',250,0,5,null,0 /* MID Track 5778 - Scheduler 'Run Now' feature gets error in audit */
exec SP_MID_SET_TEXT 080075,'Begin WorkFlow Processing',250,0,2,null,0
exec SP_MID_SET_TEXT 080076,'End   WorkFlow Processing',250,0,2,null,0
exec SP_MID_SET_TEXT 080077,'Errors found during WorkFlow Processing',250,0,5,null,0
exec SP_MID_SET_TEXT 080078,'Begin Header Processing',250,0,2,null,0
exec SP_MID_SET_TEXT 080079,'End Header Processing',250,0,2,null,0
exec SP_MID_SET_TEXT 080080,'Capacity cannot be negative',250,0,6,null,0
exec SP_MID_SET_TEXT 080081,'Cannot backout allocation since intransit is updated',250,0,6,null,0
exec SP_MID_SET_TEXT 080082,'Backout Color allocation failed',197,0,6,null,0
exec SP_MID_SET_TEXT 080083,'Backout Color allocation successful',197,0,2,null,0
exec SP_MID_SET_TEXT 080084,'Backout Pack allocation failed',197,0,6,null,0
exec SP_MID_SET_TEXT 080085,'Backout Pack allocation successful',197,0,2,null,0
exec SP_MID_SET_TEXT 080086,'Unknown allocation component',250,0,6,null,0
exec SP_MID_SET_TEXT 080087,'Reset Action invalid after shipping starts',250,0,6,null,0
exec SP_MID_SET_TEXT 080088,'Release Action successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080089,'Unknown allocation action',250,0,6,null,0
exec SP_MID_SET_TEXT 080090,'Header created successfully',250,0,2,null,0
exec SP_MID_SET_TEXT 080091,'Header deleted successfully',250,0,2,null,0
exec SP_MID_SET_TEXT 080092,'Header not found',250,0,5,null,0
exec SP_MID_SET_TEXT 080093,'Header updated successfully',250,0,2,null,0
exec SP_MID_SET_TEXT 080094,'Component is not a color component',250,0,6,null,0
exec SP_MID_SET_TEXT 080095,'Component is not a specific color component',250,0,6,null,0
exec SP_MID_SET_TEXT 080096,'Component is not a specific size component',250,0,6,null,0
exec SP_MID_SET_TEXT 080097,'Backout of Pack Allocation successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080098,'Reset Action successful:',250,0,2,null,0
exec SP_MID_SET_TEXT 080099,'Unknown Color Component',250,0,6,null,0
exec SP_MID_SET_TEXT 080100,'Unknown Size Component',250,0,6,null,0
exec SP_MID_SET_TEXT 080101,'Backout Bulk Allocation successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080102,'Backout Color/size Allocation successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080103,'Backout Detail Type allocation successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080104,'Backout Generic Type allocation successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080105,'Backout Total Allocation successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080106,'Backout Color/size Allocation failed',250,0,2,null,0
exec SP_MID_SET_TEXT 080107,'Workflow continue',250,0,2,null,0
exec SP_MID_SET_TEXT 080108,'Restart at line number exceeds number of action/method lines in workflow',250,0,5,null,0
exec SP_MID_SET_TEXT 080109,'Primary or Secondary RID must be a valid record ID',250,0,6,null,0
exec SP_MID_SET_TEXT 080110,'Designated primary is not valid primary must contain same content with at least the same receipt quantities as the designated secondary header primary cannot be currently attached to a secondary',250,0,6,null,0
exec SP_MID_SET_TEXT 080111,'Primary allocation successfully attached to secondary',250,0,2,null,0
exec SP_MID_SET_TEXT 080112,'Secondary allocation successfully attached to primary',250,0,2,null,0
exec SP_MID_SET_TEXT 080113,'Primary allocation unavailable reset not performed',250,0,5,null,0
exec SP_MID_SET_TEXT 080114,'Duplicate color/sizes not allowed',250,0,5,null,0
exec SP_MID_SET_TEXT 080115,'General Header Action successfully completed',250,0,2,null,0
exec SP_MID_SET_TEXT 080116,'Allocation action or method profile is not an allocation profile',250,0,5,null,0
exec SP_MID_SET_TEXT 080117,'Allocation process action or method not attached to allocation workflow',250,0,5,null,0
exec SP_MID_SET_TEXT 080118,'Objects in grade arraylist must be type AllocationGradeBin',250,0,6,null,0
exec SP_MID_SET_TEXT 080119,'Objects in Pack arraylist must be type PackHdr',250,0,6,null,0 
exec SP_MID_SET_TEXT 080120,'Store RID {0} not found in hashtable',250,0,6,null,0   /* MID Track 4214 identify store in error message */
exec SP_MID_SET_TEXT 080121,'Size Group not found',250,0,6,null,0
exec SP_MID_SET_TEXT 080122,'Only Drag and Drop {0} or {1} nodes',250,0,6,null,0
exec SP_MID_SET_TEXT 080123,'Method type in specified Method RID does not match specified Method Type',250,0,6,null,0
exec SP_MID_SET_TEXT 080124,'Specified Method RID is not a Fill Sizes Method',250,0,6,null,0
exec SP_MID_SET_TEXT 080125,'Reserve store not defined',250,0,6,null,0
exec SP_MID_SET_TEXT 080126,'Stores not loaded on allocation header',250,0,6,null,0
exec SP_MID_SET_TEXT 080127,'New total not spread because locked totals exceed desired new total',250,0,6,null,0
exec SP_MID_SET_TEXT 080128,'"BulkIsDetail" cannot change after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080129,'New pack already exists on header',250,0,6,null,0
exec SP_MID_SET_TEXT 080130,'Cannot change pack type after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080131,'Cannot change pack multiple after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080132,'Cannot change packs to allocate after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080133,'Bulk Color Total Summary node is Read Only for Quantity Allocated',250,0,6,null,0
exec SP_MID_SET_TEXT 080134,'Detail subtype Summary node is Read Only for Quantity Allocated',250,0,6,null,0
exec SP_MID_SET_TEXT 080135,'Expected Detail Pack when processing detail type',250,0,6,null,0
exec SP_MID_SET_TEXT 080136,'Generic Type Summary node is Read Only for Quantity Allocated',250,0,6,null,0
exec SP_MID_SET_TEXT 080137,'Type Summary node is Read Only for Quantity Allocated',250,0,6,null,0
exec SP_MID_SET_TEXT 080138,'Unknown allocation summary node',250,0,6,null,0
exec SP_MID_SET_TEXT 080139,'Type Summary node is Read Only for Quantity Shipped',250,0,6,null,0			
exec SP_MID_SET_TEXT 080140,'Generic Type Summary node is Read Only for Quantity Shipped',250,0,6,null,0			
exec SP_MID_SET_TEXT 080141,'Detail Type Summary node is Read Only for Quantity Shipped',250,0,6,null,0			
exec SP_MID_SET_TEXT 080142,'Detail Subtype Summary node is Read Only for Quantity Shipped',250,0,6,null,0			
exec SP_MID_SET_TEXT 080143,'Bulk Summary node is Read Only for Quantity Shipped',250,0,6,null,0			
exec SP_MID_SET_TEXT 080144,'Bulk Color Total Summary node is Read Only for Quantity Shipped',250,0,6,null,0
exec SP_MID_SET_TEXT 080145,'Shipping Status is "On Hold" cannot update units shipped',250,0,6,null,0
exec SP_MID_SET_TEXT 080146,'Type Summary node is Read Only for Ship Flag Value',250,0,6,null,0			
exec SP_MID_SET_TEXT 080147,'Generic Type Summary node is Read Only for Ship Flag Value',250,0,6,null,0			
exec SP_MID_SET_TEXT 080148,'Detail Type Summary node is Read Only for Ship Flag Value',250,0,6,null,0			
exec SP_MID_SET_TEXT 080149,'Detail Subtype Summary node is Read Only for Ship Flag Value',250,0,6,null,0			
exec SP_MID_SET_TEXT 080150,'Bulk Summary node is Read Only for Ship Flag Value',250,0,6,null,0			
exec SP_MID_SET_TEXT 080151,'Bulk Color Total Summary node is Read Only for Ship Flag Value',250,0,6,null,0
exec SP_MID_SET_TEXT 080152,'Generic type node does not implement Filled Size Hole Flag',250,0,6,null,0
exec SP_MID_SET_TEXT 080153,'Type Summary node does not implement Filled Size Hole Flag',250,0,6,null,0
exec SP_MID_SET_TEXT 080154,'Detail SubType Summary node does not implement Filled Size Hole Flag',250,0,6,null,0
exec SP_MID_SET_TEXT 080155,'Bulk Color Total Summary node does not implement Filled Size Hole Flag',250,0,6,null,0
exec SP_MID_SET_TEXT 080156,'Method name and type already exist for this user.',250,0,6,null,0
exec SP_MID_SET_TEXT 080157,'Method insert invalid.',250,0,6,null,0
exec SP_MID_SET_TEXT 080158,'New Bulk Color already exists on header',250,0,6,null,0
exec SP_MID_SET_TEXT 080159,'Duplicate Bulk Color not allowed',250,0,6,null,0
exec SP_MID_SET_TEXT 080160,'Duplicate Size in Pack Color not allowed',250,0,6,null,0
exec SP_MID_SET_TEXT 080161,'Duplicate Color in Pack not allowed',250,0,6,null,0
exec SP_MID_SET_TEXT 080162,'Cannot remove color from pack when packs allocated',250,0,6,null,0
exec SP_MID_SET_TEXT 080163,'Compare objects must be class HdrSizeBin',250,0,6,null,0
exec SP_MID_SET_TEXT 080164,'New Size already exists in Bulk Color',250,0,6,null,0
exec SP_MID_SET_TEXT 080165,'Compare objects must be class HdrColorBin',250,0,6,null,0
exec SP_MID_SET_TEXT 080166,'Compare objects must be class AllocationGradeBin',250,0,6,null,0
exec SP_MID_SET_TEXT 080167,'Compare objects must be class AllocationProfile',250,0,6,null,0
exec SP_MID_SET_TEXT 080168,'Duplicate Subtotal names not allowed',250,0,6,null,0
exec SP_MID_SET_TEXT 080169,'Color hierarchy node within Style not found',250,0,6,null,0
exec SP_MID_SET_TEXT 080170,'Size hierarchy node within Style-Color not found',250,0,6,null,0
exec SP_MID_SET_TEXT 080171,'Release Allocation successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080172,'Back out of Size Intransit successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080173,'Update of Size Intransit successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080174,'Back out of Style Intransit successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080175,'Update of Style Intransit successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080176,'Charge Intransit failed',250,0,6,null,0
exec SP_MID_SET_TEXT 080177,'Cancel Intransit failed',250,0,6,null,0
exec SP_MID_SET_TEXT 080178,'Cancel Size Intransit failed',250,0,6,null,0
exec SP_MID_SET_TEXT 080179,'Ship flags only valid for Total, specific pack, specific color or specific color-dimension-size',250,0,6,null,0
exec SP_MID_SET_TEXT 080180,'Specified Method RID is not a Rule Method',250,0,6,null,0
exec SP_MID_SET_TEXT 080181,'Allocation begin date cannot be changed after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080182,'Allocation grade boundaries cannot be changed after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080183,'Header style cannot be changed after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080184,'Header membership in Multi-Header cannot be changed after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080185,'Allocation on-hand source cannot be changed after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080186,'Allocation on-hand factor cannot be changed after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080187,'Allocation OTS Forecast cannot be changed after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080188,'Allocation Ship To Date cannot be changed after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080189,'Allocation Work-Up-Buy type cannot be changed after allocation has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080190,'Cannot delete header when allocation started',250,0,6,null,0
exec SP_MID_SET_TEXT 080191,'Allocation unit cost cannot be negative',250,0,6,null,0
exec SP_MID_SET_TEXT 080192,'Allocation unit retail cannot be negative',250,0,6,null,0
exec SP_MID_SET_TEXT 080193,'All Colors Units Allocated is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080194,'All Colors Rule is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080195,'All Generic Packs Allocated is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080196,'All Generic Packs Rule is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080197,'All Detail Packs Allocated is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080198,'All Detail Packs Rule is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080199,'All Packs Allocated is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080200,'All Packs Rule is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080201,'All Sizes Allocated is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080202,'Last Need Day is not valid for size',250,0,6,null,0
exec SP_MID_SET_TEXT 080203,'Minimum is not tracked for "All Sizes" component',250,0,6,null,0
exec SP_MID_SET_TEXT 080204,'Minimum is not tracked for a size across all colors',250,0,6,null,0
exec SP_MID_SET_TEXT 080205,'Rule is not tracked at size level',250,0,6,null,0
exec SP_MID_SET_TEXT 080206,'Size allocation across color is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080207,'Component Type is unknown',250,0,6,null,0
exec SP_MID_SET_TEXT 080208,'Begin WorkFlow Processing',250,0,2,null,0
exec SP_MID_SET_TEXT 080209,'End   WorkFlow Processing',250,0,2,null,0
exec SP_MID_SET_TEXT 080210,'Errors found during WorkFlow Processing',250,0,5,null,0
exec SP_MID_SET_TEXT 080211,'Begin Header Processing',250,0,2,null,0
exec SP_MID_SET_TEXT 080212,'End Header Processing',250,0,2,null,0
exec SP_MID_SET_TEXT 080213,'Get or set not configured to handle GeneralComponent',250,0,6,null,0
exec SP_MID_SET_TEXT 080214,'Percent Need Before is not implemented at size level',250,0,6,null,0
exec SP_MID_SET_TEXT 080215,'Unit Need Before is not implemented at size level',250,0,6,null,0	
exec SP_MID_SET_TEXT 080216,'Pack node does not implement Filled Size Hole Flag',250,0,6,null,0
exec SP_MID_SET_TEXT 080217,'Allocate Need not implemented for "All Detail Packs" general component',250,0,6,null,0
exec SP_MID_SET_TEXT 080218,'Allocate Need not implemented for "All Packs" general component',250,0,6,null,0
exec SP_MID_SET_TEXT 080219,'Allocate Need not valid for size',250,0,6,null,0
exec SP_MID_SET_TEXT 080220,'All Colors Total is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080221,'Detail Sub Type Total is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080222,'Type Total is read only',250,0,6,null,0
exec SP_MID_SET_TEXT 080223,'Allocate Size Proportional can only be applied to color components',250,0,6,null,0
exec SP_MID_SET_TEXT 080224,'Transaction must be an Application Session Transaction to load stores',250,0,6,null,0
exec SP_MID_SET_TEXT 080225,'Transaction cannot be changed after Allocation Profile List created',250,0,6,null,0
exec SP_MID_SET_TEXT 080226,'Duplicate Header RIDs ignored',250,0,6,null,0
exec SP_MID_SET_TEXT 080227,'Must "create" size before resetting',250,0,6,null,0
exec SP_MID_SET_TEXT 080228,'Must "create" color before resetting',250,0,6,null,0
exec SP_MID_SET_TEXT 080229,'Reserve Units is only implemented for: TOTAL, individual Packs, individual Bulk Colors, and individual Bulk Sizes',250,0,6,null,0
exec SP_MID_SET_TEXT 080230,'Size request requires both color and size be identified',250,0,6,null,0
exec SP_MID_SET_TEXT 080231,'Selected headers must treat Bulk the same',250,0,6,null,0
exec SP_MID_SET_TEXT 080232,'General Component for a subtotal spread must identify a specific pack, color, color-size or summary node (total, detail type, or bulk,0.',250,0,6,null,0
exec SP_MID_SET_TEXT 080233,'Pack is not defined on subtotal',250,0,6,null,0
exec SP_MID_SET_TEXT 080234,'Must be method to drop.',250,0,6,null,0
exec SP_MID_SET_TEXT 080235,'A method is required for this action.',250,0,6,null,0
exec SP_MID_SET_TEXT 080236,'The method type is wrong for this action.',250,0,6,null,0
exec SP_MID_SET_TEXT 080237,'The workflow has no steps.',250,0,6,null,0
exec SP_MID_SET_TEXT 080238,'Tolerance percent is not valid.',250,0,6,null,0
exec SP_MID_SET_TEXT 080239,'Component is required.',250,0,6,null,0
exec SP_MID_SET_TEXT 080240,'Review flag is not valid.',250,0,6,null,0
exec SP_MID_SET_TEXT 080241,'A valid color is required for this component',250,0,6,null,0
exec SP_MID_SET_TEXT 080242,'A valid size is required for this component',250,0,6,null,0
exec SP_MID_SET_TEXT 080243,'A valid pack is required for this component',250,0,6,null,0
exec SP_MID_SET_TEXT 080244,'An error was encountered while processing the store filter.',250,0,6,null,0
exec SP_MID_SET_TEXT 080245,'Update Intransit Failed for hierarchy node key = (0,0, day = (1,0, store RID = (2,0',150,0,6,null,0
exec SP_MID_SET_TEXT 080246,'Update Intransit Failed for Header ID = {0}',200,0,6,null,0
exec SP_MID_SET_TEXT 080247,'Packs do not update Bulk Intransit',250,0,6,null,0
exec SP_MID_SET_TEXT 080248,'An action is required.',250,0,6,null,0
exec SP_MID_SET_TEXT 080249,'Method name is not found.',250,0,6,null,0
exec SP_MID_SET_TEXT 080250,'A method is not valid for this type of action.',250,0,6,null,0
exec SP_MID_SET_TEXT 080251,'Change to header {0} component {1} could not be reflected in parent component {2} for store RID {3}',150,0,6,null,0
exec SP_MID_SET_TEXT 080252,'Spread for subtotal {0} failed on header {1}, component {2} and store RID {3}',150,0,6,null,0
exec SP_MID_SET_TEXT 080253,'Change to header {0} component {1} could not be reflected in subtotal {2} for store RID {3}',150,0,6,null,0  
exec SP_MID_SET_TEXT 080254,'Unexpected exception in allocation wafer',250,0,6,null,0
exec SP_MID_SET_TEXT 080255,'Allocation Component must be Total, Detail, Bulk, specific Pack or specific Color',250,0,6,null,0
exec SP_MID_SET_TEXT 080256,'Method RID cannot be less than 1',250,0,6,null,0
exec SP_MID_SET_TEXT 080257,'Rule Allocation Layer Already Exists',250,0,6,null,0
exec SP_MID_SET_TEXT 080258,'Rule Allocation Layer does not exist',250,0,6,null,0
exec SP_MID_SET_TEXT 080259,'Rule Layer ID cannot be less than 1',250,0,6,null,0
exec SP_MID_SET_TEXT 080260,'Entered value of: {0} cannot be less than minimum value of: {1}',250,0,6,null,0  
exec SP_MID_SET_TEXT 080261,'Entered value of: {0} cannot be greater than maximum value of: {1}',250,0,6,null,0  
exec SP_MID_SET_TEXT 080262,'Entered value of: {0} must be a multiple of: {1}',250,0,6,null,0  
exec SP_MID_SET_TEXT 080263,'Used Capacity must be greater than or equal to 0',250,0,6,null,0
exec SP_MID_SET_TEXT 080264,'Relieve Intransit component must be Total, Color or Color-Size',250,0,6,null,0
exec SP_MID_SET_TEXT 080265,'Relieve Intransit Successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080266,'Relieve Intransit Failed',250,0,6,null,0
exec SP_MID_SET_TEXT 080267,'Workflow name and type already exist for this user.',250,0,6,null,0
exec SP_MID_SET_TEXT 080268,'Duplicate HeaderID not allowed.',250,0,6,null,0
exec SP_MID_SET_TEXT 080269,'No header selected on Allocation Workspace or selected header requires Save.',250,0,4,null,0
exec SP_MID_SET_TEXT 080270,'More than one header selected on Allocation Workspace; please select only one',250,0,4,null,0
exec SP_MID_SET_TEXT 080271,'Included and Excluded Stores instruction cannot both be None.',250,0,6,null,0
exec SP_MID_SET_TEXT 080272,'Multiple cannot exceed Units to Allocate',250,0,6,null,0
exec SP_MID_SET_TEXT 080273,'Header Release requires All In Balance status',250,0,6,null,0
exec SP_MID_SET_TEXT 080274,'Header Release requires Release Approved Status',250,0,6,null,0
exec SP_MID_SET_TEXT 080275,'Cannot process this action for headers with status of {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 080276,'Please enter or drag and drop a {0} from the tree',250,0,6,null,0
exec SP_MID_SET_TEXT 080277,' {0} is not a valid {1}',250,0,6,null,0
exec SP_MID_SET_TEXT 080278,'Release Approved successful',250,0,2,null,0
exec SP_MID_SET_TEXT 080279,'Release Action failed',250,0,6,null,0
--exec SP_MID_SET_TEXT 080280,'Are you sure you want to {0}?',250,0,6,null,0	/*TT#505-MD-VStuart-Several soft text messages have the wrong message level*/
exec SP_MID_SET_TEXT 080280,'Are you sure you want to {0}?',250,0,5,null,0	/*TT#505-MD-VStuart-Several soft text messages have the wrong message level*/
exec SP_MID_SET_TEXT 080281,'Action on {0} is not allowed',250,0,6,null,0
exec SP_MID_SET_TEXT 080282,'Store list not in sync with Begin or End date list',250,0,7,null,0 
exec SP_MID_SET_TEXT 080283,'Rule *{0}* is unrecognized and cannot be processed',250,0,6,null,0
exec SP_MID_SET_TEXT 080284,' Changing {0} will cause the current Matrix data to be deleted',250,0,5,null,0
exec SP_MID_SET_TEXT 080285,'Determine Rules Failed for Header {0} Message: {1}',250,0,6,null,0
exec SP_MID_SET_TEXT 080286,'Packs not compatible for EXACT allocation rule',250,0,6,null,0
exec SP_MID_SET_TEXT 080287,'Must be an allocation method to drop.',250,0,6,null,0
exec SP_MID_SET_TEXT 080288,'Date period cannot be less than current sales week',250,0,6,null,0
exec SP_MID_SET_TEXT 080289,'Entered dates must have the same date type',250,0,6,null,0
exec SP_MID_SET_TEXT 080290,'Beginning date cannot be greater than Shipping To date',250,0,6,null,0
exec SP_MID_SET_TEXT 080291,'Store Grade Time Period cannot exceed {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 080292,'Size Multiple must evenly divide Color Multiple',250,0,6,null,0
exec SP_MID_SET_TEXT 080293,'Release Action requires that Bulk Size Allocations be balanced',250,0,6,null,0
exec SP_MID_SET_TEXT 080294,'Reset Action is not valid after shipping has started',250,0,6,null,0
exec SP_MID_SET_TEXT 080295,'Release Action requires that the Style Allocation (Packs, Bulk and Color) be balanced',250,0,6,null,0
exec SP_MID_SET_TEXT 080296,'Subtotal has no members',250,0,6,null,0
exec SP_MID_SET_TEXT 080297,'Size Multiple request requires both color and size be specified',250,0,6,null,0
exec SP_MID_SET_TEXT 080298,'Allocation Action NOT Implemented',250,0,6,null,0
exec SP_MID_SET_TEXT 080299,'Component is not in the selected header',250,0,6,null,0
exec SP_MID_SET_TEXT 080300,'Rules cannot be applied after need has occurred or intransit has been updated',250,0,6,null,0
exec SP_MID_SET_TEXT 080301,' {0} is not valid; please enter or drag and drop a node from the tree',250,0,4,null,0
exec SP_MID_SET_TEXT 080302,'Units to allocate is calculated when header is Work Up Total Buy',250,0,6,null,0
exec SP_MID_SET_TEXT 080303,'OTS Plan Level not defined',250,0,6,null,0
exec SP_MID_SET_TEXT 080304,'Component type is invalid for selected instruction',250,0,6,null,0
exec SP_MID_SET_TEXT 080305,'Instruction only valid for Color component',250,0,6,null,0
exec SP_MID_SET_TEXT 080306,'Instruction only valid for Total component',250,0,6,null,0
exec SP_MID_SET_TEXT 080307,'Unable to delete header {0} because shipping is not complete',250,0,6,null,0
exec SP_MID_SET_TEXT 080308,'No Stores have allocated units to breakout into size.',250,0,5,null,0
exec SP_MID_SET_TEXT 080309,'Changing {0} will close the Store Detail window',250,0,5,null,0
exec SP_MID_SET_TEXT 080310,'Time period begin date cannot be greater than time period end',250,0,4,null,0 
exec SP_MID_SET_TEXT 080311,' Changing {0} will require the current Matrix data to be reprocessed.',250,0,5,null,0
exec SP_MID_SET_TEXT 080312,'Balance tolerance for {0} exceeded or equaled {1}. {2} not performed',250,0,5,null,0
exec SP_MID_SET_TEXT 080313,'No units allocated by NEED action for {0} due to rules or constraints',250,0,5,null,0
exec SP_MID_SET_TEXT 080314,'There are no units remaining to be allocated for {0}',250,0,5,null,0
exec SP_MID_SET_TEXT 080315,'There is no unit need for a work up buy for {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 080316,'There are no sales and no stock plans to use to allocate {0}',250,0,5,null,0
exec SP_MID_SET_TEXT 080317,'Ship To (or Need Horizon) for the allocation of {0} cannot be found on the calendar',250,0,5,null,0
exec SP_MID_SET_TEXT 080318,'Header Release File NOT Found in Configuration File',250,0,5,null,0
exec SP_MID_SET_TEXT 080319,'Reset Action Failed:',250,0,5,null,0
exec SP_MID_SET_TEXT 080320,'Cannot trigger an undefined workflow',250,0,5,null,0
exec SP_MID_SET_TEXT 080321,'Workflow trigger failed: ',250,0,5,null,0
exec SP_MID_SET_TEXT 080322,'Workflow trigger was successful: ',250,0,2,null,0
exec SP_MID_SET_TEXT 080323,'Workflow has no headers to process',250,0,5,null,0
exec SP_MID_SET_TEXT 080324,'Workflow trigger requires that the transaction contain only the header to be triggered: ',250,0,5,null,0
exec SP_MID_SET_TEXT 080325,'Automatic backout Size Intransit: ',250,0,5,null,0
exec SP_MID_SET_TEXT 080326,'Automatic backout Style Intransit: ',250,0,5,null,0
exec SP_MID_SET_TEXT 080327,'Cannot delete released header: ',250,0,5,null,0
exec SP_MID_SET_TEXT 080328,'Automatic backout Store Allocation: ',250,0,5,null,0
exec SP_MID_SET_TEXT 080329,'Cannot release dummy header: ',250,0,5,null,0
exec SP_MID_SET_TEXT 080330,'Size Need Algorithm Failed',250,0,5,null,0
exec SP_MID_SET_TEXT 080331,'Specified Method RID is not a Basis Size Method',250,0,6,null,0
exec SP_MID_SET_TEXT 080332,'Specified Method RID is not a Size Need Method',250,0,6,null,0
exec SP_MID_SET_TEXT 080333,'{0} Total Units to Allocate changed after Allocation Started',250,0,5,null,0
exec SP_MID_SET_TEXT 080334,'{0} Color {1} Units to Allocate changed after Allocation Started',250,0,5,null,0
exec SP_MID_SET_TEXT 080335,'{0} Pack {1} Packs to Allocate changed after Allocation Started',250,0,5,null,0
exec SP_MID_SET_TEXT 080336,'{0} Color {1} Size {2} Units to Allocate changed after Allocation Started',250,0,5,null,0
exec SP_MID_SET_TEXT 080337,'{0} Cannot change Units to Allocate when Shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080338,'{0} Reset Release/Release Approved due to receipt change',250,0,5,null,0
exec SP_MID_SET_TEXT 080339,'{0} Cancel Size Intransit due to size receipt change',250,0,5,null,0
exec SP_MID_SET_TEXT 080340,'{0} Cancel Style Intransit due to receipt change',250,0,5,null,0
exec SP_MID_SET_TEXT 080341,'{0} Cannot change Packs to Allocate when Shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080342,'{0} Cannot change Units to Allocate when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080343,'{0} Cannot change Packs to Allocate when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080344,'Header enqueues failed for task {0} sequence {1}. See other messages for details',250,0,5,null,0
exec SP_MID_SET_TEXT 080345,'There are no headers for the allocation scheduler to process for task {0} sequence {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080346,'Size Group Code is required for work up size buy header {0}',250,0,5,null,0
exec SP_MID_SET_TEXT 080347,'Specified Method RID is not a Size Override Method',250,0,6,null,0
exec SP_MID_SET_TEXT 080348,'All Store List out of sync with StoreIndexRID array',250,0,6,null,0
exec SP_MID_SET_TEXT 080349,'Component is not a Specific Primary Size Dimension',250,0,5,null,0
exec SP_MID_SET_TEXT 080350,'Component is not a Specific Secondary Size Dimension Component',250,0,5,null,0
exec SP_MID_SET_TEXT 080351,'Component is not a Size Component',250,0,5,null,0
exec SP_MID_SET_TEXT 080352,'Size Curve Group uses Size Codes that are not defined on the Size Group.',250,0,5,null,0
exec SP_MID_SET_TEXT 080353,'Size Curve Group uses Size Codes that are not defined on the Size Group.  Reference to Size Group has been removed.',250,0,5,null,0
exec SP_MID_SET_TEXT 080354,'Invalid Size Code Encountered.',250,0,6,null,0
exec SP_MID_SET_TEXT 080355,'Size Review is not valid for the selected headers',250,0,6,null,0
exec SP_MID_SET_TEXT 080356,'Bulk Units to allocate is calculated when header is Work Up Bulk Buy',250,0,6,null,0
exec SP_MID_SET_TEXT 080357,'Color Units to allocate is calculated when header is Work Up Bulk Color Buy',250,0,6,null,0
exec SP_MID_SET_TEXT 080358,'Size Units to allocate is calculated when header is Work Up Size Buy',250,0,6,null,0
exec SP_MID_SET_TEXT 080359,'Store size allocated percent to store color total cannot be negative',250,0,5,null,0
exec SP_MID_SET_TEXT 080360,'Cannot release Purchase Order',250,0,5,null,0
exec SP_MID_SET_TEXT 080361,'Size row selections are invalid for this processing. Press OK to continue processing ALL sizes for this color or press CANCEL to change your selection.',250,0,5,null,0
exec SP_MID_SET_TEXT 080362,'Workflow stopped for review',250,0,5,null,0
exec SP_MID_SET_TEXT 080363,'No Matching Packs found on Header',250,0,5,null,0
exec SP_MID_SET_TEXT 080364,'No Matching Colors found on Header',250,0,5,null,0
exec SP_MID_SET_TEXT 080365,'Automatic Workflow Trigger',250,0,2,null,0
exec SP_MID_SET_TEXT 080366,'Reset for header ',250,0,2,null,0
exec SP_MID_SET_TEXT 080367,'Balance Size With Subs for header ',250,0,2,null,0
exec SP_MID_SET_TEXT 080368,'Release for header ',250,0,2,null,0
exec SP_MID_SET_TEXT 080369,'Style Need for header ',250,0,2,null,0
exec SP_MID_SET_TEXT 080370,'Begin {0} for header [{1}] ',250,0,2,null,0 /* TT#1391 - Jellis - Balance Size Constraints other options */
exec SP_MID_SET_TEXT 080371,'Balance to Reserve for header ',250,0,2,null,0
exec SP_MID_SET_TEXT 080372,'Balance Proportional for header ',250,0,2,null,0
exec SP_MID_SET_TEXT 080373,'Balance Size No Subs for header ',250,0,2,null,0
exec SP_MID_SET_TEXT 080374,'Apply Rule for header/component/layer ',250,0,2,null,0 
exec SP_MID_SET_TEXT 080375,'No basis header was declared.  Basis header is required for this processing.',250,0,6,null,0 
exec SP_MID_SET_TEXT 080376,'The target Component is not supported in Rule Processing.',250,0,6,null,0 
exec SP_MID_SET_TEXT 080377,'Basis component and target component are mismatched.',250,0,6,null,0 
exec SP_MID_SET_TEXT 080378,'Component is Invalid to be used for a basis.',250,0,6,null,0 
exec SP_MID_SET_TEXT 080379,'Packs not compatible for FILL allocation rule.',250,0,6,null,0
exec SP_MID_SET_TEXT 080380,'No Allocation was processed by Fill Sizes because no Store/Size had a positive percent Need.',250,0,5,null,0
exec SP_MID_SET_TEXT 080381,'Store {0} using size curve {1} had no sizes in the color.',250,0,5,null,0
exec SP_MID_SET_TEXT 080382,'A rule is required.',250,0,4,null,0
exec SP_MID_SET_TEXT 080383,'Headers will be ungrouped and unsorted before adding new header.  Do you wish to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 080384,'Size In-transit is already updated',250,0,2,null,0
exec SP_MID_SET_TEXT 080385,'Size In-transit not updated since size allocation does not balance',250,0,5,null,0
exec SP_MID_SET_TEXT 080386,'Size In-transit not updated since style allocation does not balance',250,0,5,null,0
exec SP_MID_SET_TEXT 080387,'Size In-transit not updated since there are no colors on header',250,0,5,null,0 
exec SP_MID_SET_TEXT 080388,'Size Review is not valid for Analysis Only',250,0,6,null,0
exec SP_MID_SET_TEXT 080389,'A large number of headers will affect performance.  Do you wish to continue?',250,0,6,null,0
exec SP_MID_SET_TEXT 080390,'The maximum number of headers has been exceeded.  Additional headers will not be displayed. Adjust filter if necessary.',250,0,6,null,0
exec SP_MID_SET_TEXT 080391,'Color Component in a Color-Size Component must be specific when defining HoldComponentAllocation',250,0,5,null,0
exec SP_MID_SET_TEXT 080392,'Size Component in a Color-Size Component must be specific when defining HoldComponentAllocation',250,0,5,null,0
exec SP_MID_SET_TEXT 080393,'Determine Soft Rule for header/component/layer ',250,0,2,null,0
exec SP_MID_SET_TEXT 080394,'Capture Allocation State before soft rule for header/component/layer ',250,0,2,null,0
exec SP_MID_SET_TEXT 080395,'Restore Allocation State before soft rule for header/component/layer ',250,0,2,null,0
exec SP_MID_SET_TEXT 080396,'Remove Soft Rule for header/component/layer ',250,0,2,null,0
exec SP_MID_SET_TEXT 080397,'Soft Rule Successfully Saved for header/component/layer ',250,0,2,null,0
exec SP_MID_SET_TEXT 080398,'Soft Rule Successfully Attached to header/component/layer ',250,0,2,null,0
exec SP_MID_SET_TEXT 080399,'There is no soft rule to save for header ',250,0,2,null,0
exec SP_MID_SET_TEXT 080400,'Soft Rule Failed to attach to header/component/layer ',250,0,2,null,0
exec SP_MID_SET_TEXT 080401,'Begin Allocate "Soft" Rule for header/component/layer ',250,0,2,null,0
exec SP_MID_SET_TEXT 080402,'End Allocate "Soft" Rule for header/component/layer ',250,0,2,null,0
exec SP_MID_SET_TEXT 080403,'Begin Allocate Rule for header/component/layer ',250,0,2,null,0
exec SP_MID_SET_TEXT 080404,'End Allocate Rule for header/component/layer ',250,0,2,null,0
exec SP_MID_SET_TEXT 080405,'Begin Process {0} {1} for header {2}',250,0,2,null,0
exec SP_MID_SET_TEXT 080406,'End Process {0} {1} for header {2}',250,0,2,null,0
exec SP_MID_SET_TEXT 080407,'Begin Interactive {0} {1} for header {2}',250,0,2,null,0
exec SP_MID_SET_TEXT 080408,'End Interactive {0} {1} for header {2}',250,0,2,null,0
exec SP_MID_SET_TEXT 080409,'Size key {0} from target Header is not on Basis Header.',250,0,5,null,0
exec SP_MID_SET_TEXT 080410,'Invalid Store Attribute sent to Size Need Algorithm.',250,0,7,null,0
exec SP_MID_SET_TEXT 080411,'Please specify a size group before defining a size curve.',250,0,4,null,0
exec SP_MID_SET_TEXT 080412,'Size curve required for Size Need Analysis.',250,0,4,null,0
exec SP_MID_SET_TEXT 080413,'Percent cannot be more than 100.',250,0,6,null,0
exec SP_MID_SET_TEXT 080414,'Only whole units can be specified.',250,0,6,null,0
exec SP_MID_SET_TEXT 080415,'{0}: {1} completed successfully.',250,0,2,null,0
exec SP_MID_SET_TEXT 080416,'{0}: {1} failed.',250,0,6,null,0
exec SP_MID_SET_TEXT 080417,'Invalid Store Attribute sent to Basis Size Algorithm.',250,0,7,null,0
exec SP_MID_SET_TEXT 080418,'Header type may not be released.',250,0,7,null,0
exec SP_MID_SET_TEXT 080419,'{0} [(1)] Target header [{2}] to process is same as Basis header. Please choose a different target header or a different basis.',250,0,7,null,0 /* MID Track 5778 - Scheduler 'Run Now' feature gets Error in Audit */
exec SP_MID_SET_TEXT 080420,'Backout Detail Pack Allocation Failed',250,0,5,null,0
exec SP_MID_SET_TEXT 080421,'Component must be Total, Detail Type, All Non-Generic Packs, Bulk, All Colors or Specific NonGeneric Pack for back out Detail Pack Allocation',250,0,5,null,0
exec SP_MID_SET_TEXT 080422,'Cannot delete component when allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080423,'Cannot Change Color name after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080424,'Sizes in size curve do not match sizes in header.',250,0,5,null,0
exec SP_MID_SET_TEXT 080425,'Size allocation is out of balance; size intransit not updated',250,0,5,null,0
exec SP_MID_SET_TEXT 080426,'Style allocation is out of balance; style intransit not updated',250,0,5,null,0
exec SP_MID_SET_TEXT 080427,'Do you want the {0} selection changed to {1}?',250,0,5,null,0
exec SP_MID_SET_TEXT 080428,'To allocate using Size Need, the component must be an individual color, bulk, all "detail" packs or the total header',250,0,5,null,0
/* (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement */
exec SP_MID_SET_TEXT 080429,'Allocated quantity cannot be negative',250,0,6,null,0
exec SP_MID_SET_TEXT 080430,'Original allocated quantity cannot be negative',250,0,6,null,0
exec SP_MID_SET_TEXT 080431,'Master {0} already assigned to subordinate {1}',250,0,6,null,0
exec SP_MID_SET_TEXT 080432,'Master {0} allocation has NOT been started',250,0,6,null,0
exec SP_MID_SET_TEXT 080433,'Master {0} still assigned to subordinate {1}',250,0,6,null,0
exec SP_MID_SET_TEXT 080434,'Master pack multiple cannot be greater than Subordinate pack multiple',250,0,6,null,0  --	issue 4061
exec SP_MID_SET_TEXT 080435,'Master pack multiple must evenly divide Subordinate pack multiple',250,0,6,null,0		--	issue 4061
exec SP_MID_SET_TEXT 080436,'Cannot process Master Total component type; use Specific component type',250,0,6,null,0
exec SP_MID_SET_TEXT 080437,' {0} header does not contain any packs',250,0,6,null,0
exec SP_MID_SET_TEXT 080438,'Number of {0} packs exceed {1} packs',250,0,6,null,0
exec SP_MID_SET_TEXT 080439,'All {0} packs could not be matched to {1} packs',250,0,6,null,0
exec SP_MID_SET_TEXT 080440,'Reserve allocated quantity cannot be negative',250,0,6,null,0
/* (CSMITH) - END MID Track #3219                                  */
exec SP_MID_SET_TEXT 080441,'Style Intransit is already charged. Balance Action not performed for header ',250,0,5,null,0
exec SP_MID_SET_TEXT 080442,'One or more stores have size curves that do not match any allocated sizes on the header.' ,250,0,5,null,0
exec SP_MID_SET_TEXT 080443,'Total store OTS need and percent need calculation failed. Arrays out of sync.', 250,0,5,null,0
exec SP_MID_SET_TEXT 080444,'Store OTS need and percent need failed. Store Need has NOT been calculated first',250,0,5,null,0
exec SP_MID_SET_TEXT 080445,' Changing {0} will cause all basis data to be deleted',250,0,5,null,0
exec SP_MID_SET_TEXT 080446,'The following selected header(s) is in use by a multi-header:',250,0,5,null,0
exec SP_MID_SET_TEXT 080447,'This header is in use by a multi-header and cannot be released:',250,0,5,null,0
exec SP_MID_SET_TEXT 080448,'Allocation Header {0} is in use by a multi-header and cannot be processed.',250,0,5,null,0
exec SP_MID_SET_TEXT 080449,'An existing Multi-Header cannot be selected when creating a new Multi-Header',250,0,5,null,0
exec SP_MID_SET_TEXT 080450,'All headers being added to a Multi-Header must have a status of {0}',250,0,5,null,0
exec SP_MID_SET_TEXT 080451,'Only one Multi-Header may be selected when adding to a Multi-Header',250,0,5,null,0
exec SP_MID_SET_TEXT 080452,'No Multi-Header selected to add to',250,0,5,null,0
exec SP_MID_SET_TEXT 080453,'No header selected to add to Multi-Header',250,0,5,null,0
exec SP_MID_SET_TEXT 080454,'Please remove column grouping before maintaining a Multi-Header',250,0,5,null,0 
exec SP_MID_SET_TEXT 080455,'Multi-header intransit split completed. Header ID: ',250,0,2,null,0 
exec SP_MID_SET_TEXT 080456,'No matching pack({0}) between multi-header({1}) and child headers.',250,0,7,null,0 
exec SP_MID_SET_TEXT 080457,'Pack {1} added to header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080458,'Cannot add Pack to header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080459,'Cannot add Pack to header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080460,'Pack {1} removed from header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080461,'Cannot remove Pack from header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080462,'Cannot remove Pack from header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080463,'Color {2} added to Pack {1} on header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080464,'Cannot add color to Pack on header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080465,'Cannot add color to Pack on header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080466,'Color {2} removed from Pack {1} on header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080467,'Cannot remove color from Pack on header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080468,'Cannot remove color from Pack on header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080469,'Color {2} changed in Pack {1} on header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080470,'Cannot change color in Pack on header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080471,'Cannot change color in Pack on header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080472,'Changed Color {2} Units in Pack {1} on header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080473,'Cannot change color units in Pack on header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080474,'Cannot change color units in Pack on header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080475,'Added Size {3} to Color {2} in Pack {1} on header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080476,'Cannot add size to Pack on header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080477,'Cannot add size to Pack on header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080478,'Removed Size {2} from Color {1} in Pack on header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080479,'Cannot remove size from Pack on header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080480,'Cannot remove size from Pack on header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080481,'Changed Size {3} in Color {2} in Pack {1} on header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080482,'Cannot change size in Pack on header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080483,'Cannot change size in Pack on header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080484,'Changed Size {3} Units in Color {2} in Pack {1} on header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080485,'Cannot change size units in Pack on header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080486,'Cannot change size units in Pack on header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080487,'Added Bulk Color {1} on header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080488,'Cannot add bulk color on header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080489,'Cannot add bulk color on header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080490,'Removed Bulk Color {1} from header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080491,'Cannot remove bulk color from header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080492,'Cannot remove bulk color from header {0} after shipping has started',250,0,5,null,0  
exec SP_MID_SET_TEXT 080493,'Added Bulk Size {2} to Color {1} on header {0} after allocation started',250,0,2,null,0 /* TT#538 - change message level from warning to information */
exec SP_MID_SET_TEXT 080494,'Cannot add bulk size to header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080495,'Cannot add bulk size to header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080496,'Removed Bulk Size {2} from Color {1} on header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080497,'Cannot remove bulk size from header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080498,'Cannot remove bulk size from header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080499,'Changed Bulk Size {2} in Color {1} on header {0} after allocation started',250,0,5,null,0
exec SP_MID_SET_TEXT 080500,'Cannot change bulk size on header {0} when status is {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080501,'Cannot change bulk size on header {0} after shipping has started',250,0,5,null,0
exec SP_MID_SET_TEXT 080502,'Duplicate colors included in the Multi-Header must match on whether or not they contain sizes',250,0,5,null,0
exec SP_MID_SET_TEXT 080503,'All headers included in the Multi-Header must match on whether or not they contain components',250,0,5,null,0
exec SP_MID_SET_TEXT 080504,'{0} Rule Method {1} cannot determine {2} when component is {3}. Component must be {4}.',250,0,5,null,0
exec SP_MID_SET_TEXT 080505,'Status of Multi-header prohibits any addition or deletion',250,0,5,null,0
exec SP_MID_SET_TEXT 080506,'Headers with packs cannot be changed to the selected type',250,0,5,null,0 
exec SP_MID_SET_TEXT 080507,'Size Constraint Model is required when using Size Minimum and Maximum Rules',250,0,5,null,0
exec SP_MID_SET_TEXT 080508,'There are no sizes in the selected color {0}',250,0,5,null,0      /* MID Tracks 3738, 3811, 3827 Status issues */
exec SP_MID_SET_TEXT 080509,'OTS Plan Level cannot be determined',250,0,5,null,0 
exec SP_MID_SET_TEXT 080510,'Size Intransit is already charged. Balance Action not performed for header ',250,0,5,null,0
exec SP_MID_SET_TEXT 080511,'Header ID “{0}” must not contain any of the following characters: {1}',250,0,5,null,0  /* MID Track 4024 Header ID contains invalid characters for file name */
exec SP_MID_SET_TEXT 080512,'(Msg Obsolete) Velocity basis cannot resolve the keyword "color" when the selected header component contains no color or more than one color',250,0,5,null,0  /* MID Track 4094 */ /* TT#1607 */
exec SP_MID_SET_TEXT 080513,'Target header to process and Prior header are the same. Please choose a different target header or a different prior header.',250,0,7,null,0
exec SP_MID_SET_TEXT 080514,'Header {0} removed as member of Multi-Header {1} due to reset action on {0}',250,0,5,null,0 /*MID Track 4033 allow update of member headers of multi */
exec SP_MID_SET_TEXT 080515,'Header {0} in use by user {1}; requested action failed for header {3}',250,0,5,null,0 /*MID Track 4033 allow update of member headers of multi */
exec SP_MID_SET_TEXT 080516,'Can not delete header {0} because of status',250,0,5,null,0  
exec SP_MID_SET_TEXT 080517,'Header {0} deleted using the Force Delete option',250,0,2,null,0  
exec SP_MID_SET_TEXT 080518,'Delete header {0}',250,0,2,null,0  
exec SP_MID_SET_TEXT 080519,'MultiHeader {0} cannot be a work up buy',250,0,5,null,0 /*MID Track 4033 allow update of members of multi header */
exec SP_MID_SET_TEXT 080520,'Delete of header {0} failed',250,0,5,null,0  
exec SP_MID_SET_TEXT 080521,'Add to Multi Header requested but no multi header id specified',250,0,5,null,0 /* MID Track 4033 allow update of members of multi Header */
exec SP_MID_SET_TEXT 080522,'Cannot add header {0} to multi header {2} when header {0} status {1}',250,0,5,null,0 /* MID Track 4033 allow update of members of multi Header */
exec SP_MID_SET_TEXT 080523,'Cannot add headers to multi header {0} when multi header status {1}',250,0,5,null,0 /* MID Track 4033 allow update of members of multi Header */
exec SP_MID_SET_TEXT 080524,'Multi Header Create requested but no multi header id specified',250,0,5,null,0 /* MID Track 4033 allow update of members of multi header */
exec SP_MID_SET_TEXT 080525,'Header {0} already belongs to multi header {1}; cannot add it to multi header {2}',250,0,5,null,0 /* MID Track 4033 allow update of members of multi header */
exec SP_MID_SET_TEXT 080526,'Header {0} must be saved before adding it to multi header {1}',250,0,5,null,0 /* MID Track 4033 allow update of members of multi headers */ 
exec SP_MID_SET_TEXT 080527,'Header {0} can not be deleted because it participates in a master or multi relationship.',250,0,5,null,0  
exec SP_MID_SET_TEXT 080528,'Size Group on Multi Header {0} does not contain all sizes of header {1}',250,0,5,null,0 /* MID Track 4033 allow update of multi headers */
exec SP_MID_SET_TEXT 080529,'Create of Multi Header {0} failed; see other audit messages',250,0,5,null,0 /* MID Track 4033 allow update of multi headers */
exec SP_MID_SET_TEXT 080530,'Add to Multi Header {0} failed; see other audit messages',250,0,5,null,0 /* MID Track 4033 allow update of multi headers */
exec SP_MID_SET_TEXT 080531,'Create of Multi Header {0} successful',250,0,2,null,0 /* MID Track 4033 allow update of multi headers */
exec SP_MID_SET_TEXT 080532,'Add to Multi Header {0} successful',250,0,2,null,0 /* MID Track 4033 allow update of multi headers */
exec SP_MID_SET_TEXT 080533,'Delete existing sizes before changing size group',250,0,5,null,0 
exec SP_MID_SET_TEXT 080534,'All size rows will be deleted',250,0,5,null,0 
exec SP_MID_SET_TEXT 080535,'The following headers are in use:',250,0,5,null,0 /* TT#4515 - stodd - enqueue message */
exec SP_MID_SET_TEXT 080536,'Only 1 component can be selected for action processing', 250,0,5,null,0 
exec SP_MID_SET_TEXT 080537,'Begin: Remove Headers from Multi-Header {0}',250,0,2,null,0 /* MID Track 4132 Workspace Blank after Remove */
exec SP_MID_SET_TEXT 080538,'End: Remove Headers from Multi-Header {0}',250,0,2,null,0 /* MID Track 4132 Workspace Blank after Remove */
exec SP_MID_SET_TEXT 080539,'Cannot "force" removal of headers from Multi {0} when there is no application session transation present',250,0,7,null,0 /* MID Track 4132 Workspace Blank after Remove */
exec SP_MID_SET_TEXT 080540,'Cannot Remove Headers from Multi-Header {0} when Shipping Started',250,0,6,null,0 /* MID Track 4132 Workspace Blank after Remove */
exec SP_MID_SET_TEXT 080541,'Cannot Remove Headers from Multi-Header {0} when {1}',250,0,6,null,0 /* MID Track 4132 Workspace Blank after Remove */
exec SP_MID_SET_TEXT 080542,'Multi-Header {0} does not contain header {1} as a child header',250,0,6,null,0 /* MID Track 4132 Workspace Blank after Remove */
exec SP_MID_SET_TEXT 080543,'Header {0} is not a Multi-Header.',250,0,6,null,0 /* MID Track 4132 Workspace Blank after Remove */
exec SP_MID_SET_TEXT 080544,'Release action failed because Header {0} was already released.',250,0,5,null,0  /* MID Track 4127 do not allow released header to be released */
exec SP_MID_SET_TEXT 080545,'Remove Headers from Multi-Header {0} Successful.',250,0,2,null,0 /* MID Track 4132 Workspace Blank after Remove */
exec SP_MID_SET_TEXT 080546,'Remove Headers from Multi-Header {0} Failed.',250,0,5,null,0 /* MID Track 4132 Workspace Blank after Remove */
exec SP_MID_SET_TEXT 080547,'Header {0} removed from Multi-Header {1}.',250,0,2,null,0 /* MID Track 4132 Workspace Blank after Remove */
exec SP_MID_SET_TEXT 080548,'You have selected {0} rows(s) for deletion', 250,0,5,null,0 
exec SP_MID_SET_TEXT 080549,'Selected header type is not allowed', 250,0,5,null,0 
exec SP_MID_SET_TEXT 080550,'Headers have changed. Please save', 250,0,5,null,0 
exec SP_MID_SET_TEXT 080551,'Size table already exists', 250,0,5,null,0 
exec SP_MID_SET_TEXT 080552,'Error retrieving size code {0}', 250,0,5,null,0 
exec SP_MID_SET_TEXT 080553,'Size code {0}, ID = {1}, not found in size group {2}', 250,0,5,null,0
exec SP_MID_SET_TEXT 080554,'Linked Header [{0}] Release Approved Successfully; Release deferred until all related Linked Headers Release Approved.',250,0,5,null,0  /* linked headers */ /* TT#TT2671 - Jellis - Release Action Failed yet Released */
exec SP_MID_SET_TEXT 080555,'Linked header characteristic must be the same for all members of a Multi-Header.',250,0,6,null,0  /* linked headers */
exec SP_MID_SET_TEXT 080556,'Linked header characteristic can not be changed.',250,0,6,null,0  /* linked headers */
exec SP_MID_SET_TEXT 080557,'Size curve: {0}  not found for Generic Size Curve parameters and header {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 080558,'Selected header {0} does not have a value for generic size characteristic {1}',250,0,6,null,0     /* MID Track 4372 Generic Size Constraints */
exec SP_MID_SET_TEXT 080559,'Selected header {0} has no bulk color defined; cannot resolve generic size name',250,0,6,null,0   /* MID Track 4372 Generic Size Constraints */
exec SP_MID_SET_TEXT 080560,'Selected header {0} has more than 1 bulk color defined; cannot resolve generic size name',250,0,6,null,0  /* MID Track 4372 Generic Size Constraints */
exec SP_MID_SET_TEXT 080561,'Size Group "{1}" missing sizes(codes) that are on header "{0}":',250,0,2,null,0 /*MID Track 4242 Allow Size Group to change */
exec SP_MID_SET_TEXT 080562,'Cannot determine Color OTS Level when header has multiple or no colors',250,0,2,null,0 /*MID Track 4020 Plan level not reset on cancel*/
exec SP_MID_SET_TEXT 080563,'Cannot determine Color OnHand Level when header has multiple or no colors',250,0,2,null,0 /*MID Track 4020 Plan level not reset on cancel*/
exec SP_MID_SET_TEXT 080564,'Begin Rebuild Intransit for Hierarchy Node {0}',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility*/
exec SP_MID_SET_TEXT 080565,'End Rebuild Intransit for Hierarchy Node {0}',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility*/
exec SP_MID_SET_TEXT 080566,'Hierarchy Node {0} not found',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility*/
exec SP_MID_SET_TEXT 080567,'There is no Parent of Style associated with Hierarchy Node {0}',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility*/
exec SP_MID_SET_TEXT 080568,'Rebuild Intransit Parent of Style {0} Headers in use by another user: ',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility*/
exec SP_MID_SET_TEXT 080569,'Rebuild Intransit failed for Hierarchy Node {0}.',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility*/
exec SP_MID_SET_TEXT 080570,'Rebuild Intransit failed for Parent of Style {0}.',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility*/
exec SP_MID_SET_TEXT 080571,'Rebuild Intransit successful for Hierarchy Node {0}',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility*/
exec SP_MID_SET_TEXT 080572,'Rebuild Intransit successful for Parent of Style {0}',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility*/
exec SP_MID_SET_TEXT 080573,'Begin Rebuild Intransit for Parent of Style {0}',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility*/
exec SP_MID_SET_TEXT 080574,'End Rebuild Intransit for Parent of Style {0}',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility*/
/* Size Constraints Batch Load Messages 080575 - 080588*/
exec SP_MID_SET_TEXT 080575,'No store groups are currently defined',250,0,6,null,0 
exec SP_MID_SET_TEXT 080576,'Model Name not defined in input file',250,0,4,null,0 
exec SP_MID_SET_TEXT 080577,'Both the Size curve and Size group values not defined for size constraint model {0}',250,0,4,null,0 
exec SP_MID_SET_TEXT 080578,'At least one Model Set should be defined for size constraint model {0}',250,0,5,null,0 
exec SP_MID_SET_TEXT 080579,'No Size Constraints transaction found in input file',250,0,4,null,0 
exec SP_MID_SET_TEXT 080580,'Size Min  or Size Mult cannot be greater thanSize Max. Size constraint model = {0}',250,0,4,null,0 
exec SP_MID_SET_TEXT 080581,'Invalid Store group level defined in size constraint model = {0}',250,0,4,null,0 
exec SP_MID_SET_TEXT 080582,'Invalid  Color Code defined in size constraint model = {0}',250,0,4,null,0 
exec SP_MID_SET_TEXT 080583,'Invalid  Dimension Code defined in size constraint model = {0}',250,0,4,null,0 
exec SP_MID_SET_TEXT 080584,'Invalid  Size Code defined in size constraint model = {0}',250,0,4,null,0 
exec SP_MID_SET_TEXT 080585,'Not all sets for all StoreGroup Locations have been defined in size constraint model = {0}',250,0,4,null,0
exec SP_MID_SET_TEXT 080586,'Invalid store group set defined in size constraint model = {0}',250,0,4,null,0
exec SP_MID_SET_TEXT 080587,'Invalid size group set defined in size constraint model  = {0}',250,0,4,null,0
exec SP_MID_SET_TEXT 080588,'Invalid size curve group set defined in size constraint model  = {0}',250,0,4,null,0
exec SP_MID_SET_TEXT 080589,'No size constraint found for generic size constraint parameters and header {0}',250,0,2,null,0   /* MID Track 4372 */
exec SP_MID_SET_TEXT 080590,'Entered filter mask does not match Global Option mask',250,0,2,null,0 
exec SP_MID_SET_TEXT 080591,'{2} rule for method {1} is not valid for header {0} when the Component is not Total',250,0,5,null,0
exec SP_MID_SET_TEXT 080592,'Ad Minimum, Allocation Minimum and Allocation Maximum rules are only valid for the Total Component.',250,0,5,null,0
exec SP_MID_SET_TEXT 080594,'API Workflow was removed successfully from header ',250,0,5,null,0
exec SP_MID_SET_TEXT 080595,'There is no API Workflow to apply to header ',250,0,5,null,0
exec SP_MID_SET_TEXT 080596,'Filter {0} has been applied to {1}: {2} for Header: {3}.',250,0,2,null,0
exec SP_MID_SET_TEXT 080597,'Apply API Workflow could not be completed for {0}. Header Intransit has already been charged.',250,0,6,null,0
exec SP_MID_SET_TEXT 080598,'Remove API Workflow could not be completed for {0}. Header allocation has already begun.',250,0,6,null,0
exec SP_MID_SET_TEXT 080599,'Do you want the previous valid node value?',250,0,2,null,0 /* MID Track 4311 */  
exec SP_MID_SET_TEXT 080600,'At least one Alternate row is required',250,0,2,null,0 /* MID Track 4970 */      
exec SP_MID_SET_TEXT 080601,'Cannot relieve intransit for header {0} whose status is not RELEASED',250,0,2,null,0 /* MID Track 5689 */
exec SP_MID_SET_TEXT 080602,'Stored Procedure [SP_MID_UPDATE_INTRANSIT] failed for header {0} with return code = ',250,0,7,null,0
exec SP_MID_SET_TEXT 080603,'[StoreIntransit] table updated successfully for header {0}. Rows affected = ',250,0,2,null,0
exec SP_MID_SET_TEXT 080604,'{0} [{1}] target header [{2}] color [{3}] has no matching color on basis header [{4}]',250,0,2,null,0 /* MID Track 5778 - Scheduler 'Run Now' feature get error in Audit */
exec SP_MID_SET_TEXT 080605,'Rule Method [{0}] may have generated unexpected rules for header [{2}] since there are no units allocated on prior header [{1}].',250,0,2,null,0 /* MID Track 5759 Give Warning when Basis Header has zero units allocated */
exec SP_MID_SET_TEXT 080606,'These headers are now excluded by the workspace filter and will be removed from the workspace: {0}',250,0,2,null,0 
exec SP_MID_SET_TEXT 080607,'Merchandise must at or above the {0} level',250,0,5,null,0  
exec SP_MID_SET_TEXT 080630,'All units allocated to reserve store. Cannot release header ',250,0,5,null,0  

exec SP_MID_SET_TEXT 080637, '{0} is designated as the Global Option Header Link Characteristic and you are not authorized to maintain that value. Delete disallowed',250,0,5,null,0
exec SP_MID_SET_TEXT 080638, '{0} is designated as the Global Option Header Link Characteristic which will also be removed',250,0,5,null,0
exec SP_MID_SET_TEXT 080639,'Header: {0}. Plan and basis grades do not match. {1} Rule is not valid.',250,0,6,null,0  
exec SP_MID_SET_TEXT 080640,'Action [{0}] cannot be performed for header [{1}] when "{2}"',250,0,5,null,0	/* TT#179 - Apply Track 6419 to 3.1 */
/* Begin TT#155 - JSmith - Size Curve Method */
exec SP_MID_SET_TEXT 080641,'Specified Method RID is not a Size Curve Method',250,0,6,null,0
/* End TT#155 - JSmith - Size Curve Method */
exec SP_MID_SET_TEXT 080642,'Characteristic: {0} not found and Auto Add parameter set to false or not specified',250,0,5,null,0
/* Begin TT#370 - JEllis - Build Packs Enhancement */
exec SP_MID_SET_TEXT 080645,'Pack pattern name is required and must be less than 51 characters in length.',250,0,5,null,0
exec SP_MID_SET_TEXT 080646,'Pack multiple must be greater than zero.',250,0,5,null,0
exec SP_MID_SET_TEXT 080647,'Maximum packs must be greater than zero.',250,0,5,null,0
exec SP_MID_SET_TEXT 080648,'Vendor pack pattern cannot contain a size run.',250,0,5,null,0
exec SP_MID_SET_TEXT 080649,'Pack pattern is read only.',250,0,5,null,0
exec SP_MID_SET_TEXT 080650,'Cannot modify maximum packs when pattern contains a size run.',250,0,5,null,0
exec SP_MID_SET_TEXT 080651,'Vendor pack pattern cannot be modified in the method.',250,0,5,null,0
exec SP_MID_SET_TEXT 080652,'Cannot explicitly modify pack multiple when pattern contains a size run; modify the size run.',250,0,5,null,0
exec SP_MID_SET_TEXT 080653,'Size run units must be greater than zero.',250,0,5,null,0
exec SP_MID_SET_TEXT 080654,'Cannot replace size in run because it results in the pack multiple being zero or negative.',250,0,5,null,0
exec SP_MID_SET_TEXT 080655,'Cannot set size value in run when pack pattern does not include a size run.',250,0,5,null,0
exec SP_MID_SET_TEXT 080656,'At least 1 pack pattern must be included in a pack combination.',250,0,5,null,0
exec SP_MID_SET_TEXT 080657,'Pack pattern combination pattern type must match the pattern type of each member pack pattern.',250,0,5,null,0
exec SP_MID_SET_TEXT 080658,'Removing vendor results in no pack combinations; must define a custom pack combination before removing vendor.',250,0,5,null,0
exec SP_MID_SET_TEXT 080659,'Vendor name was not found.',250,0,5,null,0
exec SP_MID_SET_TEXT 080660,'No combination pack patterns were found for Build Packs Method {0}.',250,0,5,null,0 /* TT#2512 - JSmith - Action Failed - Build Pack Method */
exec SP_MID_SET_TEXT 080661,'Cannot modify pack minimum order when vendor specified.',250,0,5,null,0    /* TT#787 Vendor Min Order applies only to packs */
exec SP_MID_SET_TEXT 080662,'Pack minimum order must be non-negative.',250,0,5,null,0                   /* TT#787 Vendor Min Order applies only to packs */
exec SP_MID_SET_TEXT 080663,'Cannot modify size multiple when vendor specified.',250,0,5,null,0
exec SP_MID_SET_TEXT 080664,'Size Multiple must be greater than 0.',250,0,5,null,0
exec SP_MID_SET_TEXT 080665,'Size Group and Size Curve Group are mutually exclusive.',250,0,5,null,0
exec SP_MID_SET_TEXT 080666,'Reserve quantity must be non-negative.',250,0,5,null,0
exec SP_MID_SET_TEXT 080667,'Combination "ePatternType" must be "BuildPacksMethod"',250,0,5,null,0
exec SP_MID_SET_TEXT 080668,'Build Packs Method [{0}] must be Work Up Bulk Size Buy containing exactly one color and at least 1 size. Selected header [{1}] is [{2}] containing {3} colors, {4} sizes.',250,0,5,null,0 /* End TT#370 - JEllis - Build Packs Enhancement */
/* Begin TT#438 - RMatelic - Default size curve */
exec SP_MID_SET_TEXT 080669,'No node properties default size curve defined for header {0}',250,0,6,null,0  
exec SP_MID_SET_TEXT 080670,'Default Size curve: {0} not found for header {1}',250,0,6,null,0 
/* End TT#438   */
exec SP_MID_SET_TEXT 080671,'Build Packs Method [{0}] must be Work Up Bulk Size Buy with NO Packs.  Selected header [{1}] is [{2}] containing [{3}] packs. ',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080672,'Apply selected option pack from Build Packs Method [{1}] to Work Up Buy allocation [{0}] FAILED.  ',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080673,'Successfully applied selected option pack from Build Packs Method [{1}] to Work Up Buy allocation [{0}].  ',250,0,2,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080674,'Work Up Buy allocation [{1}] reserve [{2}] exceeds Build Packs Method [{0}] desired total reserve [{3}]. Replaced desired by Work Up Buy reserve. ',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080675,'Apply selected option pack from Build Packs Method [{1}] to Work Up Buy allocation [{0}] resulted in all units [{2}] going to reserve.  ',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080676,'Build Packs Method [{1}] generated pack units [{2}] from option pack [{3}] less than minimum order [{4}] on Work Up Buy allocation [{0}]. Generated pack with multiple {5} was rejected.  ',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */

/* Begin TT#487/TT#488 - RMatelic - Default size curve */
exec SP_MID_SET_TEXT 080677,'Default size curve node not found for header: {0}',250,0,6,null,0 
exec SP_MID_SET_TEXT 080678,'Cannot determine size curve when multiple colors exist on header: {0}',250,0,6,null,0 
/* End TT#487/TT#488   */
exec SP_MID_SET_TEXT 080679,'Work Up Buy allocation [{1}] reserve [{2}] exceeds Build Packs Method [{0}] desired bulk reserve [{3}]. Replaced desired bulk by Work Up Buy reserve. ',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080680,'Work Up Buy allocation [{1}] in Build Packs Method [{0}], desired bulk reserve [{3}] exceeds desired total reserve [{2}]. Replaced total by bulk.',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080681,'Work Up Buy allocation [{1}] in Build Packs Method [{0}], desired packs reserve [{4}] exceeds desired total reserve [{2}] and bulk reserve [{3}] difference. Replaced desired packs by difference.',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080682,'No Pack Options generated for Work Up Buy allocation [{1}] in Build Packs Method [{0}].  MAKE sure at least one is selected for processing!!',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080683,'Build Packs Method [{0}] desired pack reserve units [{4}] exceeds or equals Work Up Buy allocation [{1}] pack units allocated [{3}] in pack [{2}]. All [{2}] packs sent to reserve.',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080684,'Work Up Buy allocation [{1}] bulk order [{3}] is less than Build Packs Method [{0}] minimum order [{4}] for option pack [{2}]. Bulk Size Units set to 0. ',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080685,'Size Multiple assumed to be 1 in Build Packs Method [{0}] option pack [{2}] on Work Up Buy allocation [{1}].',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080686,'Build Packs Method [{0}] option pack [{2}] has no size RID index for Work Up Buy allocaiton [{1}].',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080687,'Build Packs Method [{0}] cannot process header [{1}] when it is charged to intransit.',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */
exec SP_MID_SET_TEXT 080688,'[{0}] combination RID [{1}] does not exist.',250,0,5,null,0 /* TT#370 – Jellis – Build Packs Enhancement */

/* Begin TT#498/TT#499 - RMatelic - Default size curve */
exec SP_MID_SET_TEXT 080689,'Warning:\nChanging this value will cause the current Rule {0} information to be immediately erased. \nTo return to the original constraint information close the form without saving or updating.\nDo you wish to continue?',250,0,5,null,0 
exec SP_MID_SET_TEXT 080690,'* No size curve or size group has been selected *',250,0,2,null,0 
/* End TT#498/TT#499   */
/* Begin TT#368 - RMatelic - WorkupTotalBuy Allow sizes not in curve */
exec SP_MID_SET_TEXT 080691,'Allocation Header {0}: packs not allowed on Workup Total Buy',250,0,6,null,0 
/* End TT#368   */
exec SP_MID_SET_TEXT 080692,'Build Packs Method [{0}] generated no packs for Work Up Buy header [{1}]; check to see if all bulk units were assigned to reserve on the header or by the method.',250,0,5,null,0 /* TT#536 – Jellis – Build Packs error when no packs generated */
exec SP_MID_SET_TEXT 080693,'Build Packs Method [{0}] Combination [{2}] with Ship Variance [{3}] version [{4}], option {5}, generated no packs for Work Up Buy header [{1}] so it was dropped.',250,0,5,null,0 /* TT#535 – Jellis – Negative Reserve */
exec SP_MID_SET_TEXT 080694,'Allocation Work-Up-Buy type cannot be changed on header [{0}] when status is [{1}]',250,0,6,null,0 /* TT#368 Work up buy enhancement */
exec SP_MID_SET_TEXT 080695,'Build Packs Method [{0}] generated duplicate options [{2}] and [{3}] for header [{1}]: option [{2}] was dropped.',250,0,5,null,0 /* TT#580 - BP generates dup options */
exec SP_MID_SET_TEXT 080696,'Build Packs Method [{0}] All non-reserve bulk units for header [{1}] were packaged by prior packs in option [{2}]; pack ID [{3}] with multiple [{4}] was dropped.',250,0,5,null,0 /* TT#612 - unrelated issue */
/* Begin TT#634 - JSmith - Color rename */
exec SP_MID_SET_TEXT 080697,'Rename of color {0} on header {1} failed.',250,0,6,null,0
exec SP_MID_SET_TEXT 080698,'Multi-Header {0} deleted because all children were removed',250,0,6,null,0
/* End TT#634 */
/* BEGIN TT#618 - STodd - Allocation Override - Add Attribute Sets (#35) */
exec SP_MID_SET_TEXT 080699,'Changing the {0} will abandon any values in the Store Grade Sets below. Are you sure you want to continue?',250,0,5,null,0
/* END TT#618 - STodd - Allocation Override - Add Attribute Sets (#35) */
exec SP_MID_SET_TEXT 080700,'Increase Buy Percent must be non-negative',250,0,5,null,0 /* TT#669 - Build Packs Variance Enhancement*/
/* BEGIN TT#667 - Stodd - Pre-allocate Reserve */
exec SP_MID_SET_TEXT 080701,'The total of Reserve As Bulk and Reserve As Packs must equal Reserve units',250,0,5,null,0
/* END TT#667 - Stodd - Pre-allocate Reserve */

exec SP_MID_SET_TEXT 080702,'Header {0}: Field length of {1} exceeds maximum. Action cancelled',250,0,6,null,0	/* TT#687 MultiHeader field length error */ 
exec SP_MID_SET_TEXT 080703,'Header {0}: Only characteristics and notes can be modified',250,0,2,null,0  /* TT#712 - Multi Header Release - In Use by Multi Header */
exec SP_MID_SET_TEXT 080704,'Current date not within Execute Date Range. Workflow or Method not processed',250,0,2,null,0 /* TT#718-Tasklist for Allocation Workflow not working */
exec SP_MID_SET_TEXT 080705,'Build Packs Method [{0}] for option pack [{2}] removed all remaining bulk units from Work Up Buy allocation [{1}].',250,0,2,null,0 /* TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove bulk */
exec SP_MID_SET_TEXT 080706,'The following week(s) will be processed: {0}',250,0,2,null,0 /* TT#763 - stodd - sizedaytoweeksummary */
--Begin TT#793 - JScott - Size need successful -> get null reference error when selecting Size REview -> application goes into a loop size review never opens
exec SP_MID_SET_TEXT 080707,'No displayable Sizes found for header(s) selected.',250,0,5,null,0
--End TT#793 - JScott - Size need successful -> get null reference error when selecting Size REview -> application goes into a loop size review never opens
--Begin TT#785 - Stodd - reapply total allocation
exec SP_MID_SET_TEXT 080708,'Reapply total allocation could not be completed for {0}. Action requires header to be allocated with no intransit charged.',250,0,6,null,0
--End TT#785 - Stodd - reapply total allocation
exec SP_MID_SET_TEXT 080709,'Build Packs Method [{0}] failed for header [{1}] color [{4}] since selected pack combination [{2}] has size run [{3}] containing size [{5}] that is not on the header for that color.',250,0,5,null,0 /* TT#886 - Distinct Packs have same size runs */
exec SP_MID_SET_TEXT 080710,'{0} for header [{1}] started',250,0,2,null,0  /* TT#843 - New Action Balance Size With Constraints */  /* TT#794 New Wet Seal Balance */ /* TT#567 - Jellis - Inconsistent Status for Balance Functions */
--Begin tt#845 - Process and Apply buttons are confusing - apicchetti - 10/15/2010
exec SP_MID_SET_TEXT 080711,'Are you sure you want to apply these pack options?',250,0,6,null,0
--End tt#845 - Process and Apply buttons are confusing - apicchetti - 10/15/2010
exec SP_MID_SET_TEXT 080712,'{0} for header [{1}] color [{2}] failed due to constraints',250,0,5,null,0  /* TT#843 - New Action Balance Size With Constraints */  /* TT#1002 - Size Proportional Not Working in a Workflow */ /* TT#567 - Jellis - Inconsistent Status for Balance Functions */
exec SP_MID_SET_TEXT 080713,'{0} for header [{1}] color [{2}] was successful',250,0,2,null,0  /* TT#843 - New Action Balance Size With Constraints */  /* TT#1002 - Size Proportional Not Working in a Workflow */ /* TT#567 - Jellis - Inconsistent Status for Balance Functions */
exec SP_MID_SET_TEXT 080714,'{0} for header [{1}] ended',250,0,2,null,0  /* TT#843 - New Action Balance Size With Constraints */ /* TT#794 New Wet Seal Balance */ /* TT#567 - Jellis - Inconsistent Status for Balance Functions */
--Begin TT#967 - JSmith - Allows non style node to be associated with a header
exec SP_MID_SET_TEXT 080715,'Header must contain a valid style.',250,0,4,null,0
--End TT#967
exec SP_MID_SET_TEXT 080716,'{0} for header [{1}] failed due to constraints',250,0,5,null,0  /* TT#5494 - AGallagher - Balance Proportional Action Leaving Allocation Out of Balances */
exec SP_MID_SET_TEXT 080717,'{0} for header [{1}] was successful',250,0,2,null,0  /* TT#5494 - AGallagher - Balance Proportional Action Leaving Allocation Out of Balances */
exec SP_MID_SET_TEXT 080720,'Rebuild Intransit Parent of Style {0} Header Resources Auto Released: ',250,0,2,null,0 /*TT#1137 (MID Track 4351) Rebuild Intransit Utility */
exec SP_MID_SET_TEXT 080721,'Update failed for Header [{0}] because at {5} the resource locks no longer exist. Original locks for this update were created {4} by user [{1}] running in thread ID={2}, transaction ID={3}. ',250,0,6,null,0 /* TT#1185 – Verify ENQ before Update */
exec SP_MID_SET_TEXT 080722,'Header [{0}] locked {4} by user [{1}] running in thread ID={2}, transaction ID={3}. ',250,0,6,null,0 /*TT#1185 – Verify ENQ before Update */
exec SP_MID_SET_TEXT 080723,'Update for Header [{0}] failed at {4} because resource locks do not exist for the user [{1}] running in thread={2}, transaction={3}. ',250,0,6,null,0 /*TT#1185 – Verify ENQ before Update */
exec SP_MID_SET_TEXT 080724,'Enqueue for update failed since a selected header, related Multi, related Multi member or related Master or related Subordinate was in use by a user in another process.',250,0,6,null,0  /* TT#1185 – Verify ENQ before Update */
exec SP_MID_SET_TEXT 080725,'Updates failed for following headers due to enqueue issues (see other messages): ',250,0,6,null,0  /*TT#1185 – Verify ENQ before Update */
exec SP_MID_SET_TEXT 080726,'Updates failed for header(s). See audit log for details. ',250,0,6,null,0  /*TT#1185 – Verify ENQ before Update */
exec SP_MID_SET_TEXT 080727,'When {1} checked, value is required and must be {0} or less',250,0,4,null,0  /*TT#1365 – Jellis – FL Detail Pack Size Nedd Enhancement */
exec SP_MID_SET_TEXT 080729,'Inventory Min/Max for header [{0}] requires an inventory basis',250,0,6,null,0  /* TT#1287 – Inventory Minimum and Maximum */
exec SP_MID_SET_TEXT 080730,'Allocation Min/Max for header [{0}] cannot have an inventory basis specified',250,0,6,null,0  /* TT#1287 – Inventory Minimum and Maximum */
exec SP_MID_SET_TEXT 080731,'Grade Min/Max for header [{0}] successfully designated as Allocation Min/Max',250,0,2,null,0  /* TT#1287 – Inventory Minimum and Maximum */
exec SP_MID_SET_TEXT 080732,'Grade Min/Max for header [{0}] successfully designated as Inventory Min/Max',250,0,2,null,0  /* TT#1287 – Inventory Minimum and Maximum */
exec SP_MID_SET_TEXT 080733,'Begin: [{1}] Need Action for [{0}]',250,0,2,null,0  /* TT#488 - MD – JEllis – Group Allocation */
exec SP_MID_SET_TEXT 080734,'End: [{1}] Need Action for [{0}]',250,0,2,null,0  /* TT#488 - MD – JEllis – Group Allocation */
exec SP_MID_SET_TEXT 080735,'[{2}] Cannot resolve [{1}] keyword "{3}" when the selected header [{0}] component contains no color or more than one color',250,0,6,null,0  /* TT#1607 – Jellis – Inventory Basis wrong when more than one color */
/* Begin TT#1641 - RMatelic - Warning message should be sent when applying a Header Characteristic to a Multi-Header */
exec SP_MID_SET_TEXT 080736,'Warning: Multi-Header Characteristics are not applied to individual In-Use headers, only the Multi-Header',250,0,5,null,0  
/* End TT#1641 */
exec SP_MID_SET_TEXT 080737,'{0} cannot be negative',250,0,6,null,0 /* TT#1401 – JEllis – Urban Reservation Stores pt 1 */
exec SP_MID_SET_TEXT 080738,'Stored Procedure [SP_MID_UPDATE_IMO] failed for header {0} with return code = ',250,0,7,null,0
exec SP_MID_SET_TEXT 080739,'[STORE IMO] table updated successfully for header {0}. Rows affected = ',250,0,2,null,0
exec SP_MID_SET_TEXT 080740,'Update IMO Failed for Header ID = {0}',200,0,6,null,0
exec SP_MID_SET_TEXT 080741,'{0} must be between 0 and 1 (percent must be converted—ie. Divided by 100)',250,0,6,null,0 /* TT#1401 – JEllis – Urban Reservation Stores pt 1 */
exec SP_MID_SET_TEXT 080742,'Allocation Header {0} VSW is required for VSW type headers.',250,0,7,null,0 /* TT#1401 – STodd – Urban Reservation Stores */
exec SP_MID_SET_TEXT 080743,'Allocation Header {0} VSW cannot be included on non-VSW type headers. VSW will be set to null.',250,0,5,null,0 /* TT#1401 – STodd – Urban Reservation Stores */
exec SP_MID_SET_TEXT 080744,'Allocation Header {0} VSW does not currently exist and cannot be used for a VSW Header.',250,0,6,null,0 /* TT#2225 - AGallagher - VSW Modifcations Enhancement */
exec SP_MID_SET_TEXT 080745,'Header Type {0} is unknown.',250,0,7,null,0 /*  TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28B */
exec SP_MID_SET_TEXT 080746,'Item Max must have a value.',250,0,7,null,0 /*	TT#2083 - GTaylor - VSW not holding values */

/* put following message in sequence ABOVE...*/
/* Begin TT#1641 - RMatelic - Warning message should be sent when applying a Header Characteristic to a Multi-Header */
/* exec SP_MID_SET_TEXT 080736,'Warning: Multi-Header Characteristics are not applied to individual In-Use headers, only the Multi-Header',250,0,5,null,0 */ 
/* End TT#1641 */
/* begin TT#2155 - JEllis - Fill Size Holes Null Reference */
exec SP_MID_SET_TEXT 080747,'Header [{0}] has a begin date.  Fill Size Method [{1}] has no curves. So "future" onhands cannot be calculated; so Fill Size Method cannot be processed for this header.',250,0,7,null,0
exec SP_MID_SET_TEXT 080748,'Header [{0}] has a begin date.  Size Need Method [{1}] has no curves. So "future" onhands cannot be calculated; so Size Need Method cannot be processed for this header.',250,0,7,null,0
--exec SP_MID_SET_TEXT 080749,'Size Need Rules for Header [{0}] have been successfully applied',250,0,7,null,0	/*TT#505-MD-VStuart-Several soft text messages have the wrong message level*/
exec SP_MID_SET_TEXT 080749,'Size Need Rules for Header [{0}] have been successfully applied',250,0,2,null,0	/*TT#505-MD-VStuart-Several soft text messages have the wrong message level*/
exec SP_MID_SET_TEXT 080750,'Fill Size Rules for Header [{0}] have been successfully applied',250,0,7,null,0
/* end TT#2155 - JEllis - Fill Size Holes Null Reference */
/* begin TT#586 - MD - Jellis - Assortment - Add 2 dimension spread */
exec SP_MID_SET_TEXT 080761,'Spread failed: No rows to process; source [{0}/{1}]',250,0,7,null,0
exec SP_MID_SET_TEXT 080762,'Spread failed: No columns to process; source [{0}/{1}]',250,0,7,null,0
exec SP_MID_SET_TEXT 080763,'Spread failed: Row vector [{0}] count [{1}] not equal Matrix [{4}] row count [{5}] or Col vector [{2}] count [{3}] not equal Matrix [{4}] col count[{6}]; source [{7}/{8}]',250,0,7,null,0
/* end TT#586 - MD- Jellis - Assortment - Add 2 dimentsion spread */
exec SP_MID_SET_TEXT 080764,'There are no headers to process; source [{0}/{1}]',250,0,5,null,0 /* TT#488 – MD – Jellis – Group Allocation */
exec SP_MID_SET_TEXT 080765,'No valid header(s) selected in {0}.',250,0,4,null,0 /* TT#696-MD - Stodd - add "active process" */
exec SP_MID_SET_TEXT 080766,'The following selected header(s) belong to a group allocation. To process, open the group allocation.',250,0,4,null,0  /* TT#1019 - MD - stodd - prohibit allocation actions against GA */
exec SP_MID_SET_TEXT 080767,'Header(s) cannot be added to a group allocation when allocation has already started against the group.',250,0,4,null,0  /* TT#1053 - MD - stodd - drop headers when allocation started */
exec SP_MID_SET_TEXT 080768,'Header(s) cannot be removed from a group allocation when allocation has already started against the group.',250,0,4,null,0  /* TT#1053 - MD - stodd - drop headers when allocation started */
exec SP_MID_SET_TEXT 080769,'Process As "Headers" is selected, but no headers have been selected. Press "OK" to open all headers or "Cancel" to return and select specific headers.',250,0,4,null,0  /* TT#1087 - MD - stodd - size review showing wrong headers  */
exec SP_MID_SET_TEXT 080770,'Get Successful',250,0,2,null,0  /* TT#1176 - MD - Jellis – Group Allocation Size Need not observing Inventory Min Max */
/* Begin TT#1222-MD - stodd - When opening Size Review when Group Allocation is in "Group" mode and the matrix tab, size review gets a "No Displayable Sizes" error. */
exec SP_MID_SET_TEXT 080771,'Group Allocation Prohibited in Size Review',250,0,5,null,0
exec SP_MID_SET_TEXT 080772,'Displaying the Group Allocation information in Size Review is prohibited. To see header information, change "Process As" to "Headers" and select the headers you wish to view in Size Review.',250,0,5,null,0
/* End TT#1222-MD - stodd - When opening Size Review when Group Allocation is in "Group" mode and the matrix tab, size review gets a "No Displayable Sizes" error. */
/* Begin  TT#1334-MD - stodd - Balance to VSW Action */
/* Begin  TT#1405-MD - stodd - message have wrong message level */
exec SP_MID_SET_TEXT 080774,'Balance to VSW for header [{0}] started',250,0,2,null,0
exec SP_MID_SET_TEXT 080775,'it belongs to a Group Allocation or an Assortment',250,0,6,null,0
/* End TT#1334-MD - stodd - Balance to VSW Action */
exec SP_MID_SET_TEXT 080776,'Action [{0}] for component [{2}] on header [{1}] failed. (Source [{3}/{4}])',250,0,2,null,0           /* TT#1334 – Urban – Jellis – VSW Balance Enhancement */
exec SP_MID_SET_TEXT 080777,'Action [{0}] for component [{2}] on header [{1}] was successful. (Source [{3}/{4}])',250,0,2,null,0   /* TT#1334 – Urban – Jellis – VSW Balance Enhancement */
exec SP_MID_SET_TEXT 080778,'Balance to VSW for header [{0}] ended',250,0,2,null,0		/* TT#1334-MD - stodd - Balance to VSW Action */
exec SP_MID_SET_TEXT 080779,'Balance to VSW changed Store Qty on header [{0}] for component [{1}] because Store Qty Allocated was greater than receipts',250,0,2,null,0             /* TT#1396-MD - jellis – Qty Cannot be Negative */
/* End  TT#1405-MD - stodd - message have wrong message level */
exec SP_MID_SET_TEXT 080780,'Found a matching released header for this Reset transaction. Also found {0} matching non-released header(s). Reset is rejected because it will cause duplicate non-released headers.',250,0,6,null,0  /* TT#1649-MD - stodd - Change Header matching logic to look at "Shipping Started" indicator for a Reset transaction */
exec SP_MID_SET_TEXT 080781,'Units Per Carton must be an even multiple of the header Multiple',250,0,5,null,0 /* TT#1652-MD - RMatelic - DC Carton Rounding */
exec SP_MID_SET_TEXT 080782,'Cannot update Units Per Carton on header {0} when status is {1}',250,0,5,null,0 /* TT#1652 - RMatelic – DC Carton rounding */
exec SP_MID_SET_TEXT 080783,'Cannot update Units Per Carton on header {0} when type is VSW or in Assortment or Group Alocation',250,0,5,null,0 /* TT#1652 - RMatelic – DC Carton rounding */
/* Begin TT#1652-MD - stodd - DC Carton Rounding */
exec SP_MID_SET_TEXT 080784,'DC Carton Rounding cannot be processed against a header where intransit has been charged',250,0,6,null,0 
exec SP_MID_SET_TEXT 080785,'DC Carton Rounding is not supported for this type header',250,0,6,null,0 
exec SP_MID_SET_TEXT 080786,'DC Carton Rounding is not supported for headers with sizes',250,0,6,null,0 
exec SP_MID_SET_TEXT 080787,'DC Carton Rounding cannot be process on a header with more than one pack',250,0,6,null,0 
exec SP_MID_SET_TEXT 080788,'DC Carton Rounding cannot be process on a header with more than one color',250,0,6,null,0 
exec SP_MID_SET_TEXT 080789,'DC Carton Rounding cannot be process on a header with packs and bulk colors defined',250,0,6,null,0 
exec SP_MID_SET_TEXT 080790,'For DC Carton Rounding, total units must be evenly divisible by the pack multiple',250,0,6,null,0 
exec SP_MID_SET_TEXT 080791,'For DC Carton Rounding, Units Per Carton must be evenly divisible by the allocation multiple',250,0,6,null,0 
exec SP_MID_SET_TEXT 080792,'No Attribute Set in the chosen DC Carton Rounding store attribute matches the DC on the header. Cannot determine receiving DC Attribute Set.',250,0,6,null,0 
exec SP_MID_SET_TEXT 080793,'For DC Carton Rounding, the Receiving DC has no stores included in the matching Attribute Set.',250,0,6,null,0 
/* End TT#1652-MD - stodd - DC Carton Rounding */
exec SP_MID_SET_TEXT 080794,'For DC Carton Rounding, Units Per Carton is zero.',250,0,6,null,0 /* TT#1692-MD - stodd - process should not run if Units Per Carton has not been assigned */
exec SP_MID_SET_TEXT 080795,'DC Carton Rounding cannot be processed against a header where the stores include VSW IDs. VSW is not supported.',250,0,6,null,0 /* TT#1693-MD - stodd - DC Carton Rounding - add edit to stop processing if any stores have a VSW ID defined. */
exec SP_MID_SET_TEXT 080796,'DC Carton Rounding successfully completed for method, {0}, on header: {1}',250,0,2,null,0	/* TT#1699-MD - stodd - DC Carton Rounding is missing an information message stating it was run against a specific header */
exec SP_MID_SET_TEXT 080797,'Units Per Carton is invalid for header: {0}',250,0,5,null,0 /* 1703-MD - stodd - Error when Units Per Carton field is blank */
exec SP_MID_SET_TEXT 080798,'For DC Carton Rounding, the Receiving DC has no stores allocated in the matching Attribute Set.',250,0,6,null,0 /* TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance */
exec SP_MID_SET_TEXT 080799,'One or more headers already belong to a Group Allocation.',250,0,6,null,0 /* TT#1753-MD - stodd - Object reference error charging intransit on multi-header */
exec SP_MID_SET_TEXT 080800,'One or more headers already belong to an Assortment.',250,0,6,null,0 /* TT#1753-MD - stodd - Object reference error charging intransit on multi-header */
/* Begin  TT#1334-MD - stodd - Balance to VSW Action */
exec SP_MID_SET_TEXT 080801,'Begin: Remove Headers from Group {0}',250,0,2,null,0 /* TT#4755 - JSmith - Cannot Remove header from Group */
exec SP_MID_SET_TEXT 080802,'End: Remove Headers from Group {0}',250,0,2,null,0 /* TT#4755 - JSmith - Cannot Remove header from Group */
exec SP_MID_SET_TEXT 080803,'Cannot "force" removal of headers from Group {0} when there is no application session transation present',250,0,7,null,0 /* TT#4755 - JSmith - Cannot Remove header from Group */
exec SP_MID_SET_TEXT 080804,'Cannot Remove Headers from Group {0} when Shipping Started',250,0,6,null,0 /* TT#4755 - JSmith - Cannot Remove header from Group */
exec SP_MID_SET_TEXT 080805,'Cannot Remove Headers from Group {0} when {1}',250,0,6,null,0 /* TT#4755 - JSmith - Cannot Remove header from Group */
exec SP_MID_SET_TEXT 080806,'Group {0} does not contain header {1} as a child header',250,0,6,null,0 /* TT#4755 - JSmith - Cannot Remove header from Group */
exec SP_MID_SET_TEXT 080807,'Header {0} is not a Group.',250,0,6,null,0 /* TT#4755 - JSmith - Cannot Remove header from Group */
exec SP_MID_SET_TEXT 080808,'Remove Headers from Group {0} Successful.',250,0,2,null,0 /* TT#4755 - JSmith - Cannot Remove header from Group */
exec SP_MID_SET_TEXT 080809,'Remove Headers from Group {0} Failed.',250,0,5,null,0 /* TT#4755 - JSmith - Cannot Remove header from Group */
exec SP_MID_SET_TEXT 080810,'Header {0} removed from Group {1}.',250,0,2,null,0 /* TT#4755 - JSmith - Cannot Remove header from Group */
exec SP_MID_SET_TEXT 080811,'Summary Review is not valid when Group Allocation is open and no headers are selected.',250,0,6,null,0	/* TT#4793 - stodd - GA- Heades are different styles-> Velocity basis is color-> get Vel basis error-> accidentally select Summary review-> get Index out of range-> select ok and attemt to cancel group-> get Null reference error. */
exec SP_MID_SET_TEXT 080812,'DC Carton Rounding cannot be processed against a header where intransit has been charged.  Method has been skipped while processing a workflow.',250,0,5,null,0 
/* Begin TT#1966-MD - JSmith- DC Fulfillment */
exec SP_MID_SET_TEXT 080813,'Action on {0} is not allowed. DC Fulfillment has been processed.',250,0,5,null,0 
exec SP_MID_SET_TEXT 080814,'Action on {0} is not allowed. DC Fulfillment has not been processed.  Cannot update subordinate headers.',250,0,5,null,0
exec SP_MID_SET_TEXT 080815,'Cannot release master header: ',250,0,5,null,0
exec SP_MID_SET_TEXT 080816,'Must be master header: ',250,0,5,null,0
exec SP_MID_SET_TEXT 080817,'DC Fulfillment completed. Header ID: ',250,0,2,null,0 
exec SP_MID_SET_TEXT 080818,'Store Order entry not found for DC. Stores will be sequenced in key order.',250,0,2,null,0 
exec SP_MID_SET_TEXT 080819,'Duplicate DC entries found. The first entry will be used.',250,0,5,null,0 
exec SP_MID_SET_TEXT 080820,'No matching pack({0}) between master-header({1}) and subordinate headers.',250,0,6,null,0 
exec SP_MID_SET_TEXT 080821,'Unable to apply minimums to all stores in master-header({0}).',250,0,5,null,0 
exec SP_MID_SET_TEXT 080822,'Unable to apply DC Fulfillment to all stores in master-header({0}).',250,0,6,null,0 
exec SP_MID_SET_TEXT 080823,'Unable to apply DC Fulfillment to all stores from their primary DC in master-header({0}).  Process will spread units across DCs.',250,0,2,null,0 
exec SP_MID_SET_TEXT 080824,'DC Fulfillment already processed on master-header.',250,0,5,null,0 
exec SP_MID_SET_TEXT 080825,'Cannot delete master-header after DC Fulfillment: ',250,0,5,null,0
exec SP_MID_SET_TEXT 080826,'Cannot delete subordinate header: ',250,0,5,null,0
exec SP_MID_SET_TEXT 080827,'Master Header Intransit must be charged for both Style/Color and Size (or none) before processing the DC Fulfillment Method.',250,0,5,null,0
exec SP_MID_SET_TEXT 080828,'At least one entry is required if Use Selected Headers is not checked.',250,0,5,null,0
exec SP_MID_SET_TEXT 080829,'All subordinate headers must have a DC assigned to process DC Fulfillment.',250,0,6,null,0
exec SP_MID_SET_TEXT 080830,'DC Fulfillment has been processed.',250,0,5,null,0 
exec SP_MID_SET_TEXT 080831,'DC Fulfillment has not been processed.  Cannot update subordinate headers.',250,0,5,null,0
exec SP_MID_SET_TEXT 080832,'Master header cannot be relieved.  Relieve subordinate headers.',250,0,5,null,0
exec SP_MID_SET_TEXT 080833,'Action on header {0} is not valid because header is either a master or a subordinate.',250,0,5,null,0
exec SP_MID_SET_TEXT 080834,'One or more headers already belong to a Master Header.',250,0,6,null,0
exec SP_MID_SET_TEXT 080835,'{0} already belongs to a Master Header and cannot be added to the Group Allocation.',250,0,5,null,0
exec SP_MID_SET_TEXT 080836,'Must have a DC {0} assigned to process DC Fulfillment.',250,0,6,null,0
/* End TT#1966-MD - JSmith- DC Fulfillment */
exec SP_MID_SET_TEXT 080837,'The following Worklist Items are in use:',250,0,5,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 080838,'Selected GroupBy is not implemented for Style View',250,0,7,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 080839,'Selected GroupBy is not implemented for Size View',250,0,7,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 080840,'Selected GroupBy is not implemented for Summary View',250,0,7,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 080841,'Selected View is not Implemented',250,0,7,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 080842,'Color already contains sizes',250,0,6,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 080843,'Count must be greater than zero to add placeholders',250,0,6,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 080844,'Count must be greater than zero to add placeholder colors',250,0,6,null,0   /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 080845,'Style Review must be closed before Interactive mode can be enabled.',250,0,5,null,0   /* RO-3444 - JSmith - RO Web Messaging */

/* Assortment Messages 085000-089999 */
exec SP_MID_SET_TEXT 085000,'Variable, {0}, not supported by General Assortment Method', 250,0,7,null,0 
exec SP_MID_SET_TEXT 085001,'Cannot perform Redo. No previous General Assortment has been run.', 250,0,7,null,0 
exec SP_MID_SET_TEXT 085002,'An existing Assortment cannot be selected when creating a new Assortment', 250,0,5,null,0 
exec SP_MID_SET_TEXT 085003,'All headers being added to an Assortment must have a status of {0}', 250,0,5,null,0
exec SP_MID_SET_TEXT 085004,'Only one {0} or Placeholder row may be selected when adding to an {0}', 250,0,5,null,0	/*TT#1395-MD - stodd - Adding VSW Headers to a Group produces incorrect messages */
exec SP_MID_SET_TEXT 085005,'Status of {0} row prohibits any addition or deletion', 250,0,5,null,0
exec SP_MID_SET_TEXT 085006,'Cannot add headers to Assortment row when Placeholders exist', 250,0,5,null,0
exec SP_MID_SET_TEXT 085007,'No Assortment or Placeholder selected to add to', 250,0,5,null,0
exec SP_MID_SET_TEXT 085008,'No header selected to add to Assortment', 250,0,5,null,0
exec SP_MID_SET_TEXT 085009,'All headers being added to the Placeholder must match the Placeholder style', 250,0,5,null,0
exec SP_MID_SET_TEXT 085010,'Entry of Placeholder color is not allowed', 250,0,5,null,0
exec SP_MID_SET_TEXT 085011,'View no longer exists. List will be rebuilt', 250,0,5,null,0 
exec SP_MID_SET_TEXT 085012,'{0} view: {1} will be deleted', 250,0,5,null,0
exec SP_MID_SET_TEXT 085013,'Can not close style due to computation error', 250,0,5,null,0
exec SP_MID_SET_TEXT 085014,'Assortment Review Message',250,0,5,null,0
exec SP_MID_SET_TEXT 085015,'Duplicate {0} not allowed within an Assortment', 250,0,5,null,0
exec SP_MID_SET_TEXT 085016,'Anchor node must be at a higher hierarchy level', 250,0,5,null,0
exec SP_MID_SET_TEXT 085017,' {0} will be changed to {1}', 250,0,5,null,0
exec SP_MID_SET_TEXT 085018,'Placeholder anchor node must be a descendent of the Assortment anchor node', 250,0,5,null,0
exec SP_MID_SET_TEXT 085019,'Select only 1 row for Save As', 250,0,5,null,0
exec SP_MID_SET_TEXT 085020,'Multiple headers added to a single Placeholder must have the same {0}', 250,0,5,null,0
/* BEGIN TT#2150 - stodd - totals not showing in main matrix grid */
exec SP_MID_SET_TEXT 085021,'Placeholder', 250,0,2,null,0
/* BEGIN TT#2150 - stodd - totals not showing in main matrix grid */
exec SP_MID_SET_TEXT 085022,'No Assortment selected to process', 250,0,5,null,0
exec SP_MID_SET_TEXT 085023,'Duplicate colors included in the same Style/SKU must match on whether or not they contain sizes', 250,0,5,null,0
exec SP_MID_SET_TEXT 085024,'All headers with the same Style/SKU must match on whether or not they contain components', 250,0,5,null,0
exec SP_MID_SET_TEXT 085025,'All selected headers must be in an Assortment for this option', 250,0,5,null,0 
exec SP_MID_SET_TEXT 085026,'Do you want the non-inherited lower level(s) to inherit the changed characteristic(s)?', 250,0,5,null,0 
exec SP_MID_SET_TEXT 085027,'Placeholder cannot be maintained if Released, Intransit or Charged to Committed', 250,0,5,null,0
exec SP_MID_SET_TEXT 085028,'No headers selected to process', 250,0,5,null,0 
/* BEGIN TT#1936 - stodd - cancel allocation/assortment */
/* BEGIN TT#1936 - stodd - changed message */
exec SP_MID_SET_TEXT 085029,'No headers/placeholders selected to process or all selected headers/placeholders are invalid for the method or action selected', 250,0,6,null,0	/* TT#1559-MD - stodd - msg level incorrect */
/* END TT#1936 - stodd - changed message */
/* END TT#1936 - stodd - cancel allocation/assortment */

/* BEGIN TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load */
exec SP_MID_SET_TEXT 085030,'Allocation Header  {0} Total Units must be > 0', 250,0,6,null,0
exec SP_MID_SET_TEXT 085031,'Allocation Header  {0} Pack {1} Color Code ID {2} Units must be > 0', 250,0,6,null,0
exec SP_MID_SET_TEXT 085032,'Allocation Header  {0} Pack {1} Size Code ID {2} Units must be > 0', 250,0,6,null,0
exec SP_MID_SET_TEXT 085033,'Allocation Header  {0} Color Code ID {1} Units must be > 0', 250,0,6,null,0
exec SP_MID_SET_TEXT 085034,'Allocation Header  {0} Size Code ID {1} Units must be > 0', 250,0,6,null,0
exec SP_MID_SET_TEXT 085035,'Allocation Header  {0} Color Code ID {1} Size Code ID {2} Units must be > 0', 250,0,6,null,0
exec SP_MID_SET_TEXT 085036,'Allocation Header  {0} Pack {1} Color Code ID {2} Size Code ID {3} Units must be > 0', 250,0,6,null,0
/* END TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load */

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
exec SP_MID_SET_TEXT 085037,'Size Curve Transaction # {0} has NO Size Curve Group Name', 250,0,7,null,0
exec SP_MID_SET_TEXT 085038,'Size Curve Group {0} (Transaction #"  {1}) - Begin Processing', 250,0,2,null,0
exec SP_MID_SET_TEXT 085039,'Transaction: Size Curve Group Name: [{0}] ', 250,0,5,null,0
exec SP_MID_SET_TEXT 085040,'Message: Size Curve Group is NOT defined', 250,0,5,null,0
exec SP_MID_SET_TEXT 085041,'Message: No store transactions found to process', 250,0,4,null,0
exec SP_MID_SET_TEXT 085042,'Size Curve Group {0} (Transaction #"  {1}) - End Processing', 250,0,2,null,0
exec SP_MID_SET_TEXT 085043,'No stores are currently defined', 250,0,7,null,0
exec SP_MID_SET_TEXT 085044,'No Size Curve Group transactions found in input file', 250,0,2,null,0
exec SP_MID_SET_TEXT 085045,'Groups Read: {0} Groups In Error: {1} Groups Loaded: {2}', 250,0,0,null,0
exec SP_MID_SET_TEXT 085046,'Transaction: Size Curve Group Name: [{0}] Store ID: [{1}] Size Curve Name: [{2}]', 250,0,2,null,0
exec SP_MID_SET_TEXT 085047,'Message: Store transaction is empty', 250,0,4,null,0
exec SP_MID_SET_TEXT 085048,'Message: Store ID is required', 250,0,4,null,0
exec SP_MID_SET_TEXT 085049,'Message: Store ID is NOT defined', 250,0,4,null,0
exec SP_MID_SET_TEXT 085050,'Message: Size Curve Name NOT specified - Store assigned to default Size Curve Name', 250,0,2,null,0
exec SP_MID_SET_TEXT 085051,'Message: Size Curve Name does NOT exist in Size Curve Group - Store size curve assignment NOT changed', 250,0,2,null,0
exec SP_MID_SET_TEXT 085052,'Size Curve Group Name {0} (Transaction #{1} successfully processed', 250,0,2,null,0
exec SP_MID_SET_TEXT 085053,'     One or more store transactions encountered edit errors', 250,0,2,null,0
exec SP_MID_SET_TEXT 085054,' encountered an update error', 250,0,2,null,0
exec SP_MID_SET_TEXT 085055,' encountered an update error: {0}', 250,0,2,null,0
exec SP_MID_SET_TEXT 085056,'No product categories are currently defined', 250,0,7,null,0
exec SP_MID_SET_TEXT 085057,'No Size Curve Group transactions found in input file', 250,0,2,null,0
exec SP_MID_SET_TEXT 085058,'Groups Read: {0} Groups In Error: {1} Groups Created: {2}  "Groups Modified: {3} Groups Removed: {4}', 250,0,2,null,0
exec SP_MID_SET_TEXT 085059,'Size Curve Group Transaction # {1}) has NO Size Curve Group Name', 250,0,2,null,0
exec SP_MID_SET_TEXT 085060,'Size Curve Group {0} (Transaction # {1} - Begin Processing', 250,0,2,null,0
exec SP_MID_SET_TEXT 085061,'Transaction: Size Curve Group Name: [{0}],  Size Curve Group Action: [{1}]', 250,0,2,null,0
exec SP_MID_SET_TEXT 085062,'Message: Size Curve Group Action is required', 250,0,2,null,0
exec SP_MID_SET_TEXT 085063,'Message: Size Curve Group Action is invalid', 250,0,2,null,0
exec SP_MID_SET_TEXT 085064,'Message: Action invalid when Size Curve Group already defined', 250,0,2,null,0
exec SP_MID_SET_TEXT 085065,'Message: No size curve transactions found to process', 250,0,2,null,0
exec SP_MID_SET_TEXT 085066,'Message: Action invalid when Size Curve Group is NOT defined', 250,0,2,null,0
exec SP_MID_SET_TEXT 085067,'Size Curve Group {0} (Transaction # {1} - End Processing', 250,0,2,null,0
exec SP_MID_SET_TEXT 085068,'Transaction: Size Curve Group Name: [{0}] Size Curve Group Action: [{1}] Size Curve Name: [{2}] Size Curve Action: [{3}]', 250,0,2,null,0
exec SP_MID_SET_TEXT 085069,'Message: Size Curve Action is missing - defaulting to Create', 250,0,2,null,0
exec SP_MID_SET_TEXT 085070,'Message: Size Curve Action is invalid - defaulting to Create', 250,0,2,null,0
exec SP_MID_SET_TEXT 085071,'Message: Size Action is missing - defaulting to  Create', 250,0,2,null,0
exec SP_MID_SET_TEXT 085072,'Message: Size Action is invalid - defaulting to  Create', 250,0,2,null,0
exec SP_MID_SET_TEXT 085073,'Code: [{0}],', 250,0,2,null,0
exec SP_MID_SET_TEXT 085074,'Product Category: [{0}], Primary: [{1}], Secondary: [{2}],', 250,0,2,null,0
exec SP_MID_SET_TEXT 085075,'Size Action: [{0}], Value: [{1}],', 250,0,2,null,0
exec SP_MID_SET_TEXT 085076,'Message: Size already present with the following criteria {0} Code ID: [{1}], Product Category: [{2}], Primary: [{3}], Secondary: [{4}]', 250,0,2,null,0
exec SP_MID_SET_TEXT 085077,' Value has invalid value - setting to 0.00', 250,0,2,null,0
exec SP_MID_SET_TEXT 085078,'Message: No size transactions found to process', 250,0,2,null,0
exec SP_MID_SET_TEXT 085079,'Message: Error equalizing size values to 100%', 250,0,2,null,0
exec SP_MID_SET_TEXT 085080,'Message: No size transactions found to process', 250,0,2,null,0
exec SP_MID_SET_TEXT 085081,'Message: Error equalizing default curve size values to 100%', 250,0,2,null,0
exec SP_MID_SET_TEXT 085082,'Size Curve Group Name {0} (Transaction # {1}) successfully created', 250,0,2,null,0
exec SP_MID_SET_TEXT 085083,'Size Curve Group Name {0} (Transaction # {1}) encountered edit errors', 250,0,2,null,0
exec SP_MID_SET_TEXT 085084,'Message: Size Curve Action is required', 250,0,2,null,0
exec SP_MID_SET_TEXT 085085,'Message: Size Curve Action is invalid', 250,0,2,null,0
exec SP_MID_SET_TEXT 085086,'Message: Action invalid when default Size Curve already defined', 250,0,2,null,0
exec SP_MID_SET_TEXT 085087,'Message: Action invalid when Size Curve already defined', 250,0,2,null,0
exec SP_MID_SET_TEXT 085088,'Message: Size Curve removed by earlier transaction - restored from REMOVE list', 250,0,2,null,0
exec SP_MID_SET_TEXT 085089,'Message: Size already selected with the following criteria {0} Code ID: [{1}], Product Category: [{2}], Primary: [{3}], Secondary: [{4}]', 250,0,2,null,0
exec SP_MID_SET_TEXT 085090,'Message: All sizes removed from Size Curve - Size Curve removed from Size Curve Group', 250,0,2,null,0
exec SP_MID_SET_TEXT 085091,'Message: Default Size Curve can NOT be removed', 250,0,2,null,0
exec SP_MID_SET_TEXT 085092,'Size Curve Group Name {0} (Transaction # {1})  successfully modified', 250,0,2,null,0
exec SP_MID_SET_TEXT 085093,'Size Curve Group Name {0} (Transaction # {1})  encountered edit errors', 250,0,2,null,0
exec SP_MID_SET_TEXT 085094,'Size Curve Group Name {0} (Transaction # {1})', 250,0,2,null,0
exec SP_MID_SET_TEXT 085095,' encountered an update error', 250,0,2,null,0
exec SP_MID_SET_TEXT 085096,' encountered an update error: ', 250,0,2,null,0
exec SP_MID_SET_TEXT 085097,'Message: Code ID is required', 250,0,2,null,0
exec SP_MID_SET_TEXT 085098,'Message: Code ID is NOT defined', 250,0,2,null,0
exec SP_MID_SET_TEXT 085099,'Message: Product Category is required', 250,0,2,null,0
exec SP_MID_SET_TEXT 085100,'Message: Product Category is NOT defined', 250,0,2,null,0
exec SP_MID_SET_TEXT 085101,'Message: Primary (and Secondary) is required', 250,0,2,null,0
exec SP_MID_SET_TEXT 085102,'Message: Primary / Secondary is NOT defined', 250,0,2,null,0
exec SP_MID_SET_TEXT 085103,'Message: Primary / Secondary not defined in Product Category', 250,0,2,null,0
exec SP_MID_SET_TEXT 085104,'Message: No sizes defined in Product Category', 250,0,2,null,0
exec SP_MID_SET_TEXT 085105,'Message: Value cannot be negative - setting to 0.00', 250,0,2,null,0
exec SP_MID_SET_TEXT 085106,'Message: Action invalid when default Size Curve is NOT defined', 250,0,2,null,0
exec SP_MID_SET_TEXT 085107,'Message: Action invalid when Size Curve is NOT defined', 250,0,2,null,0
exec SP_MID_SET_TEXT 085108,'Message: Size Action is required', 250,0,2,null,0
exec SP_MID_SET_TEXT 085109,'Message: Size Action is invalid', 250,0,2,null,0
/*End  TT780 - FIX - RBeck - Change hard coded messages to soft text */
/* BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders */
exec SP_MID_SET_TEXT 085110,'Current View not valid for a Post-Receipt Assortment. The view will revert to the Default Post Receipt View.', 250,0,5,null,0
/* END TT#490-MD - stodd -  post-receipts should not show placeholders */
/*BEGIN TT#600-MD - stodd - Dragging a header with  pack(s) does not properly adjust the placeholder */
/* Begin TT#705-MD - stodd - removing dropping/removing header with packs does not adjust PH qty */
/* exec SP_MID_SET_TEXT 085111,'There are color components on the selected header(s) that cannot be matched with specific placeholder colors or generic placeholder colors.', 250,0,5,null,0 */
exec SP_MID_SET_TEXT 085111,'There is a mismatch between the colors on the selected header(s) and the placeholder. Either the header has no colors and the placeholder does or the header colors cannot be matched with the placeholder colors to adjust the placeholder''s quantity.', 250,0,5,null,0
/* End TT#705-MD - stodd - removing dropping/removing header with packs does not adjust PH qty */
/*END TT#600-MD - stodd - Dragging a header with  pack(s) does not properly adjust the placeholder */
/* BEGIN TT#488-MD - Stodd - Group Allocation */
exec SP_MID_SET_TEXT 085112,'{0} was not found.', 250,0,5,null,0
exec SP_MID_SET_TEXT 085113,'Selected header(s) do not belong to a Group Allocation.', 250,0,5,null,0
exec SP_MID_SET_TEXT 085114,'Selected header(s) do not belong to an Assortment.', 250,0,5,null,0
/* END TT#488-MD - Stodd - Group Allocation */
/* BEGIN TT#742-MD - Stodd - Assortment tooltips */
/* Begin TT#1395-MD - stodd - Adding VSW Headers to a Group produces incorrect messages */
exec SP_MID_SET_TEXT 085115,'Invalid header for {0}. Header already belongs to another assortment or group.', 250,0,5,null,0
exec SP_MID_SET_TEXT 085116,'Invalid header for {0}. Either the status is not Received in Balance or it is a Multi-header, a VSW header, or a WorkUp Total Buy.', 250,0,5,null,0	/* TT#4116 - stodd - prohibit dropping of VSW header */
/* End TT#1395-MD - stodd - Adding VSW Headers to a Group produces incorrect messages */
exec SP_MID_SET_TEXT 085117,'Placeholder cannot accept header. Either header is released or placeholder security prevents the addition of the header.', 250,0,5,null,0
/* END TT#742-MD - Stodd - Assortment tooltips */
/* TT#691 - Validation issue on Administration-Models-FWOS Override  RBeck */
exec SP_MID_SET_TEXT 085118,'Invalid date range.', 250,0,6,null,0
/* begin TT#488 - MD - Jellis - Group Allocation */
exec SP_MID_SET_TEXT 085119,'Invalid access of "ClearAssortment()" method by header [{0}]. Only an AssortmentProfile may clear the assortment.', 250,0,7,null,0
exec SP_MID_SET_TEXT 085120,'Assortment/Group Allocation [{0}] cannot be cleared when units [{1}] are allocated to non-reserve stores.', 250,0,5,null,0
/* end TT#488 - MD - Jellis - Group Allocation */
/*TT#806-MD-DOConnell-Assortment Placeholder on Content Tab-> ignores all criteria when a general method is processed - seems to use allocation defaults */
exec SP_MID_SET_TEXT 085121,'Cannot process General Allocation Method on an Assortment Placeholder', 250,0,5,null,0
exec SP_MID_SET_TEXT 085122,'This Group Allocation contains no headers and will be deleted. Press OK to continue with delete. Press CANCEL to return to the Group Allocation.', 250,0,5,null,0	/* TT#936 - MD - stodd - Prevent the saving of empty Group Allocations */
/* Begin TT#941 - MD - stodd - When creating a new Group Allocation from headers selected on the allocation workspace, the selected headers are not getting placed into the Group Allocation. */
exec SP_MID_SET_TEXT 085123,'Do you want to add the selected headers to the new Group Allocation?', 250,0,5,null,0	
exec SP_MID_SET_TEXT 085124,'"{0}" is not "Received in Balance" and cannot be added to the Group Allocation."', 250,0,5,null,0	
exec SP_MID_SET_TEXT 085125,'"{0}" is a Work Up Buy, VSW, Multi-header, or part of a Master Header and cannot be added to the Group Allocation.', 250,0,5,null,0		/* TT#4116 - stodd - prohibit dropping of VSW header */
exec SP_MID_SET_TEXT 085126,'"{0}" already belongs to an Assortment or Group Allocation and cannot be added to the Group Allocation.', 250,0,5,null,0	
exec SP_MID_SET_TEXT 085127,'The following headers cannot be added to the group allocation. Create Group Allocation with the remaining header(s)?\n\n ', 250,0,5,null,0	
exec SP_MID_SET_TEXT 085128,'Create Group Allocation', 250,0,5,null,0
/* End TT#941 - MD - stodd - When creating a new Group Allocation from headers selected on the allocation workspace, the selected headers are not getting placed into the Group Allocation. */	
exec SP_MID_SET_TEXT 085129,'Group Allocation method cannot be processed against individual headers. Change "Process As" in Group Allocation View to equal "Group".', 250,0,6,null,0	/* TT#1030 - MD - stodd - GA method error message */
exec SP_MID_SET_TEXT 085130,'{0}, {1}, is read only. Methods cannot be processed against it.', 250,0,6,null,0   /*TT#1033 - md - stodd - Able to process Locked Group Allocations - */
exec SP_MID_SET_TEXT 085131,'User does not have authority to run methods against {0}.', 250,0,6,null,0   /*TT#1007 - md - stodd - GA security - */
/* begin TT#1037 - MD - stodd - read only security - */
exec SP_MID_SET_TEXT 085132,'One or more selected headers belong to a Group Allocation and can only be maintained within the Group Allocation Review screen. Do you wish to continue with the selected view as read-only?', 250,0,2,null,0
exec SP_MID_SET_TEXT 085133,'Headers Belong to a Group Allocation.', 250,0,13,null,0
exec SP_MID_SET_TEXT 085134,'Cannot process an Allocation Overrider Method against a Group Allocation. Change "Process As" in Group Allocation View to equal "Headers".', 250,0,6,null,0	/* TT#1060 - MD - stodd - GA method error message */
exec SP_MID_SET_TEXT 085135,'At least one header must to be selected to create a new group allocation.', 250,0,6,null,0 /* TT#1122 - md - stodd - calendar exception creating group allocation -  */
/* end TT#1037 - MD - stodd - read only security - */
exec SP_MID_SET_TEXT 085136,'No valid headers were selected to create the new group allocation.', 250,0,6,null,0	/* TT#1122 - md - stodd - calendar exception creating group allocation -  */
/* begin TT#1601 - MD - Jellis - General Method cannot be processed against Group Allocation or any of its members */
--exec SP_MID_SET_TEXT 085137,'Cannot process a General Allocation Method against headers within a Group Allocation. Change "Process As" in Group Allocation View to equal "Group".', 250,0,6,null,0 /* TT#1139 - MD - stodd - Gen Alloc method error message */
exec SP_MID_SET_TEXT 085137,'Cannot process a General Allocation Method against a Group Allocation or any Group Allocation member header. Process a Group Allocation Method against the Group Allocation as a "Group".', 250,0,6,null,0 
/* end TT#1601 - MD - Jellis - General Method cannot be processed against Group Allocation or any of its members */exec SP_MID_SET_TEXT 085138,'All stores have been out''d by an Out Rule.', 250,0,2,null,0    /* TT#1204-MD - stodd Provide a message within Matrix when there are no stores available to spread to  */	
/* Begin TT#1395-MD - stodd - Adding VSW Headers to a Group produces incorrect messages */
exec SP_MID_SET_TEXT 085139,'{0}: {1} Loading saved Infragistics Layout.', 250,0,2,null,0 
exec SP_MID_SET_TEXT 085140,'{0}: {1} Loading default Infragistics Layout.', 250,0,2,null,0 
/* End TT#1395-MD - stodd - Adding VSW Headers to a Group produces incorrect messages */
exec SP_MID_SET_TEXT 085141,'This basis date range is set to be dynamic to the Delivery Date. Please enter a Delivery Date and try again.', 250,0,6,null,0	/* TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review  */
exec SP_MID_SET_TEXT 085142,'Action is invalid. The Store Attribute must equal the Store Attribute the assortment was created with. This action cannot be processed using a different Store Attribute.', 250,0,6,null,0	 /*TT#1530-MD - stodd - Changing Attribute in Matrix and processing Create Placeholders (& Spread Avg) should give message to user  */
exec SP_MID_SET_TEXT 085143,'Cannot process Method on Placeholder when forecast level is set at Style or below', 250,0,6,null,0		/* TT#1569-MD - stodd - new message */
/* Begin TT#1595-MD - stodd - Batch Comp */
exec SP_MID_SET_TEXT 085144,'There are no Batch Comp calculations defined.', 250,0,6,null,0 
exec SP_MID_SET_TEXT 085145,'Batch Comp: {0} is not defined.', 250,0,6,null,0 
/* Begin TT#1595-MD - stodd - Batch Comp */
/* Begin TT#1652-MD - RMatelic - DC Carton Rounding */ 
exec SP_MID_SET_TEXT 085155,'Entered value of: {0} must be evenly divide into: {1}',250,0,6,null,0  
/* End TT#1652-MD */
exec SP_MID_SET_TEXT 085156,'Cannot process a General Allocation Method against an Assortment member header.', 250,0,6,null,0 
exec SP_MID_SET_TEXT 085157,'Action not valid on placeholder', 250,0,5,null,0	/* TT#2117-MD - JSmith - Placeholder on an Assortment should not allow Need to be processed */
exec SP_MID_SET_TEXT 085158,'Cannot remove color from Post Receipt Assortment',250,0,6,null,0   /* RO-4253 - JSmith - Data transport to remove Colors from an Initial Buy Assortment Style - Content */

/* Planning Messages 090000-099999 */
exec SP_MID_SET_TEXT 090000,'Forecast in use',250,0,6,null,0
exec SP_MID_SET_TEXT 090001,'Forecast in use Please reselect',250,0,6,null,0
exec SP_MID_SET_TEXT 090002,'Cube not defined',250,0,6,null,0
exec SP_MID_SET_TEXT 090003,'Cube type not determined',250,0,6,null,0
exec SP_MID_SET_TEXT 090004,'Cell is read-only',250,0,6,null,0
exec SP_MID_SET_TEXT 090005,'Cell is display-only',250,0,6,null,0
exec SP_MID_SET_TEXT 090006,'Cell is protected',250,0,6,null,0
exec SP_MID_SET_TEXT 090007,'Cell is closed',250,0,6,null,0
exec SP_MID_SET_TEXT 090008,'Cell is ineligible',250,0,6,null,0
exec SP_MID_SET_TEXT 090009,'Cell is locked',250,0,6,null,0
exec SP_MID_SET_TEXT 090010,'Invalid call',250,0,6,null,0
exec SP_MID_SET_TEXT 090011,'Invalid SetCell mode',250,0,6,null,0
exec SP_MID_SET_TEXT 090012,'Cell not available: {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 090013,'Invalid value format',250,0,6,null,0
exec SP_MID_SET_TEXT 090014,'Forecast Review Message',250,0,6,null,0
exec SP_MID_SET_TEXT 090015,'Formula Conflict',250,0,6,null,0
exec SP_MID_SET_TEXT 090016,'Circular Reference',250,0,6,null,0
exec SP_MID_SET_TEXT 090017,'Cell is Temp-locked',250,0,6,null,0
exec SP_MID_SET_TEXT 090018,'Invalid date type on DateRangeProfile',250,0,6,null,0
exec SP_MID_SET_TEXT 090019,'Invalid number of indices',250,0,6,null,0
exec SP_MID_SET_TEXT 090020,'Invalid logical coordinate',250,0,6,null,0
exec SP_MID_SET_TEXT 090021,'Dimension not defined on cube',250,0,6,null,0
exec SP_MID_SET_TEXT 090022,'Invalid total relationship',250,0,6,null,0
exec SP_MID_SET_TEXT 090023,'Invalid Chain/Store code',250,0,6,null,0
exec SP_MID_SET_TEXT 090024,'You must select a store attribute',250,0,6,null,0
exec SP_MID_SET_TEXT 090025,'You must select a merchandise for the store',250,0,6,null,0
exec SP_MID_SET_TEXT 090026,'You must select a version for the store',250,0,6,null,0
exec SP_MID_SET_TEXT 090027,'You must select a time period',250,0,6,null,0
exec SP_MID_SET_TEXT 090028,'You must select a merchandise for the chain',250,0,6,null,0
exec SP_MID_SET_TEXT 090029,'You must select a version for the chain',250,0,6,null,0
exec SP_MID_SET_TEXT 090030,'Unfinished Basis Details detected.  You must finish filling out the entire row or delete it.',250,0,6,null,0
exec SP_MID_SET_TEXT 090031,'You must finish filling out the previous row',250,0,6,null,0
exec SP_MID_SET_TEXT 090032,'You must select a view',250,0,6,null,0
exec SP_MID_SET_TEXT 090033,'You must select a time period for the store',250,0,6,null,0
exec SP_MID_SET_TEXT 090034,'You must select a time period for the chain',250,0,6,null,0
exec SP_MID_SET_TEXT 090035,'You must specify a view name',250,0,6,null,0
exec SP_MID_SET_TEXT 090036,'The view specified already exists',250,0,6,null,0
exec SP_MID_SET_TEXT 090037,'Date requested is beyond maximum timeline',250,0,6,null,0
exec SP_MID_SET_TEXT 090038,'The following forecast(s) are in use:',250,0,6,null,0
exec SP_MID_SET_TEXT 090039,'Type: {0}, Node: {1}, Version: {2}, Week: {3}, User: {4}',250,0,6,null,0
exec SP_MID_SET_TEXT 090040,'Type: {0}, Node: {1}, Version: {2}, Weeks: {3}-{4}, User: {5}',250,0,6,null,0
exec SP_MID_SET_TEXT 090041,'Do you wish to continue with these forecasts as read-only?',250,0,6,null,0
exec SP_MID_SET_TEXT 090042,'Please select different forecast criteria.',250,0,6,null,0
exec SP_MID_SET_TEXT 090043,'Input not valid',250,0,6,null,0
exec SP_MID_SET_TEXT 090044,'Not Lockable',250,0,6,null,0
exec SP_MID_SET_TEXT 090045,'Values for this column have not completed loading.  Please try again later.',250,0,5,null,0
exec SP_MID_SET_TEXT 090046,'Values for this row have not completed loading.  Please try again later.',250,0,5,null,0
exec SP_MID_SET_TEXT 090047,'Store and chain plans have changed.  Save changes now?',250,0,2,null,0
exec SP_MID_SET_TEXT 090048,'Store plan has changed.  Save changes now?',250,0,2,null,0
exec SP_MID_SET_TEXT 090049,'Chain plan has changed.  Save changes now?',250,0,2,null,0
exec SP_MID_SET_TEXT 090050,'The Plan Heirarchy Node was not specified.',250,0,5,null,0
exec SP_MID_SET_TEXT 090051,'A Basis Heirarchy Node was not specified.',250,0,5,null,0
exec SP_MID_SET_TEXT 090052,'The plan forecast version was not specified.',250,0,5,null,0
exec SP_MID_SET_TEXT 090053,'No Attributes Sets were marked to Plan.',250,0,5,null,0
exec SP_MID_SET_TEXT 090054,'Too many columns selected to export to Excel.  Change your selection and try again.',250,0,5,null,0
exec SP_MID_SET_TEXT 090055,'Monitoring was set to TRUE, but there was a problem creating the log file.  Method will continue, but no monitoring will be done.',250,0,5,null,0
exec SP_MID_SET_TEXT 090056,'You are not authorized to update the specified Store Hierarchy Node.',250,0,5,null,0
exec SP_MID_SET_TEXT 090057,'You are not authorized to update the specified Chain Hierarchy Node.',250,0,5,null,0
exec SP_MID_SET_TEXT 090058,'The computations are not loaded or loaded incorrectly.  Contact System Support.',250,0,5,null,0
exec SP_MID_SET_TEXT 090059,'Low levels are not selected.',250,0,5,null,0
exec SP_MID_SET_TEXT 090060,'Expected Size Plan to be calculated prior to calculating Size Need. It was not.',250,0,6,null,0
exec SP_MID_SET_TEXT 090061,'Missing SizeNeedAlgorithm.',250,0,6,null,0
exec SP_MID_SET_TEXT 090062,'You must select a merchandise for the high-level store',250,0,6,null,0
exec SP_MID_SET_TEXT 090063,'You must select a merchandise for the high-level chain',250,0,6,null,0
exec SP_MID_SET_TEXT 090064,'You must select a version for the high-level store',250,0,6,null,0
exec SP_MID_SET_TEXT 090065,'You must select a version for the high-level chain',250,0,6,null,0
exec SP_MID_SET_TEXT 090066,'You must select a time period for the high-level store',250,0,6,null,0
exec SP_MID_SET_TEXT 090067,'You must select a time period for the high-level chain',250,0,6,null,0
exec SP_MID_SET_TEXT 090068,'You must select a version for the low-level store',250,0,6,null,0
exec SP_MID_SET_TEXT 090069,'You must select a version for the low-level chain',250,0,6,null,0
exec SP_MID_SET_TEXT 090070,'You must select a time period for the low-level store',250,0,6,null,0
exec SP_MID_SET_TEXT 090071,'You must select a time period for the low-level chain',250,0,6,null,0
exec SP_MID_SET_TEXT 090072,'Values have changed for {0}.  Save changes now?',250,0,2,null,0
exec SP_MID_SET_TEXT 090073,'Plan and basis time period lengths must be the same',250,0,2,null,0
exec SP_MID_SET_TEXT 090074,'Number of values copied: {0} Set: {1}',250,0,2,null,0
exec SP_MID_SET_TEXT 090075,'Set contained no stores to copy. Set: {0}',250,0,5,null,0
exec SP_MID_SET_TEXT 090076,'Total number of values copied: {0}',250,0,2,null,0
exec SP_MID_SET_TEXT 090077,'At least one Basis Detail is required',250,0,6,null,0
exec SP_MID_SET_TEXT 090078,'Invalid computations requested: {0}',250,0,6,null,0
/* Begin Track #4168 - JScott - Error on Store Mulit-level screen when specifying no low-levels */
exec SP_MID_SET_TEXT 090079,'No Low Levels exist for requested level',250,0,6,null,0
/* End Track #4168 - JScott - Error on Store Mulit-level screen when specifying no low-levels */
/* Begin Abercrombie & Fitch - JScott - Base changes - Part 19 */
exec SP_MID_SET_TEXT 090080,'No cells found to spread to',250,0,6,null,0
/* End Abercrombie & Fitch - JScott - Base changes - Part 19 */
/* BEGIN Issue 4535 - 8.1.07 */
exec SP_MID_SET_TEXT 090081,'Empty store list.',250,0,6,null,0
exec SP_MID_SET_TEXT 090082,'No stores returned from store filter.',250,0,6,null,0
exec SP_MID_SET_TEXT 090083,'No nodes were selected for processing in method: {0}.',250,0,5,null,0
/* END Issue 4535 - 8.1.07 */
exec SP_MID_SET_TEXT 090084,'The {0} version is a combination of other versions and will be opened as read only.',250,0,2,null,0
exec SP_MID_SET_TEXT 090085,'A basis is required for this option.',250,0,4,null,0  /* Issue 4233 */
exec SP_MID_SET_TEXT 090086,'There are errors in the low levels.',250,0,4,null,0  /* Issue 4858 */
exec SP_MID_SET_TEXT 090087,'You are not authorized to plan the highlighted information.',250,0,4,null,0  /* Issue 4858 */

/* BEGIN Issue 4535 - 8.1.07 */

exec SP_MID_SET_TEXT 090088,'Weight value must be greater than zero',250,0,2,null,0  /* ANF - Weighting Multiple Basis */
exec SP_MID_SET_TEXT 090089,'The sum of all basis equalized weights must equal 1',250,0,2,null,0 /* ANF - Weighting Multiple Basis */
exec SP_MID_SET_TEXT 090090,'Number of basis weeks exceeds number of high level weeks',250,0,2,null,0 /* ANF - Weighting Multiple Basis */
/* Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes */
exec SP_MID_SET_TEXT 090091,'You do not have authority to access the Chain Plan Merchandise.',250,0,4,null,0
exec SP_MID_SET_TEXT 090092,'You do not have authority to access the Store Plan Merchandise.',250,0,4,null,0
exec SP_MID_SET_TEXT 090093,'You do not have authority to access the Chain Basis Merchandise.',250,0,4,null,0
exec SP_MID_SET_TEXT 090094,'You do not have authority to access the Store Basis Merchandise.',250,0,4,null,0
/* End Track #5091 - JScott - Secuirty Lights don't change when permission changes */
/*Begin Track #5239 - JScott - Error when opening Chain with Store-only view */
exec SP_MID_SET_TEXT 090095,'The View selected does not have any displayable variables.',250,0,4,null,0
/*End Track #5239 - JScott - Error when opening Chain with Store-only view */
/* Begin Track #5115 - KJohnson - Multi Level Spread */
exec SP_MID_SET_TEXT 090096,'From levels are not selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 090097,'To levels are not selected.',250,0,4,null,0
/* End Track #5115 - KJohnson - Multi Level Spread */
--Begin TT#824 - JScott - Add Toolbox methods to support external node references
exec SP_MID_SET_TEXT 090098,'Invalid NodeID specified in computations: {0}',250,0,6,null,0
--End TT#824 - JScott - Add Toolbox methods to support external node references
--Begin TT#83 MD - JSmith - Null reference error if Chain Set Percent values and no weeks to plan
exec SP_MID_SET_TEXT 090099,'No weeks to Plan. Sales not balanced.',250,0,2,null,0
exec SP_MID_SET_TEXT 090100,'No weeks to Plan. Stock not balanced.',250,0,2,null,0
--End TT#83 MD - JSmith - Null reference error if Chain Set Percent values and no weeks to plan
--Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options; change edit
exec SP_MID_SET_TEXT 090101,'The chart requires unit variables to be displayed on this screen.',250,0,4,null,0
--End TT#1748-MD
/* Allocation Methods Messages 100000-109999 */

exec SP_MID_SET_TEXT 100000,'Method Name required please enter.',250,0,6,null,0
exec SP_MID_SET_TEXT 100001,'Method Description required please enter.',250,0,6,null,0
exec SP_MID_SET_TEXT 100002,'Please select either User or Global',250,0,6,null,0

/* Filter Messages 110000-110999 */

exec SP_MID_SET_TEXT 110000,'Filter profile type does not match list profile type',250,0,6,null,0
exec SP_MID_SET_TEXT 110001,'A Filter with the name you specified already exists.  Please specify a different name.',250,0,6,null,0
exec SP_MID_SET_TEXT 110002,'Filter Name must be entered.',250,0,6,null,0
/*Begin Track #5111 - JScott - Add additional filter functionality */
exec SP_MID_SET_TEXT 110003,'Date Range is not valid when using Join.',250,0,6,null,0
/*End Track #5111 - JScott - Add additional filter functionality */
/*Begin Assortment Planning - JScott - Assortment Planning Changes */
exec SP_MID_SET_TEXT 110004,'Filter contains data-filtering information, which will be ignored.  Is this OK?',250,0,2,null,0
/*End Assortment Planning - JScott - Assortment Planning Changes */
/*Begin Track #5727 - Stodd  */
exec SP_MID_SET_TEXT 110005,'Unable to resolve filter: {0}',250,0,2,null,0
/*End Track #5727 - Stodd  */
/*Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders */
exec SP_MID_SET_TEXT 110006,'A Folder with the name you specified already exists.  Please specify a different name.',250,0,6,null,0
/*End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders */

/* Begin TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition */
exec SP_MID_SET_TEXT 110008,'Please provide a valid filter name.',250,0,6,null,0
exec SP_MID_SET_TEXT 110009,'Filter Name Invalid',250,0,6,null,0
exec SP_MID_SET_TEXT 110010,'Filter Name Already Exists',250,0,6,null,0
exec SP_MID_SET_TEXT 110011,'Apply changes to condition?',250,0,6,null,0
exec SP_MID_SET_TEXT 110012,'Apply Changes?',250,0,6,null,0
/* End TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition */
/* Begin TT#1388-MD -jsobek -Prouct Filters */
exec SP_MID_SET_TEXT 110013,'Are you sure you want to delete the selected filter?',250,0,6,null,0
exec SP_MID_SET_TEXT 110014,'Please confirm',250,0,6,null,0
exec SP_MID_SET_TEXT 110015,'Please select or create a filter first.',250,0,6,null,0
/* End TT#1388-MD -jsobek -Prouct Filters */
/* Begin TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number */
exec SP_MID_SET_TEXT 110016,'Copying Task with User Filter to Global or System Folder Not Allowed',250,0,6,null,0
exec SP_MID_SET_TEXT 110017,'Copying an allocate task with a user filter to a global or system folder is not allowed.',250,0,6,null,0
/* End TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number */
/* Begin TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 110018,'Attributes must have one set defined.',250,0,6,null,0
exec SP_MID_SET_TEXT 110019,'Invalid Attribute Filter',250,0,6,null,0
exec SP_MID_SET_TEXT 110020,'Duplicate set names are not permitted.',250,0,6,null,0
exec SP_MID_SET_TEXT 110021,'Invalid Attribute Filter',250,0,6,null,0
exec SP_MID_SET_TEXT 110022,'Cannot use the same field or characteristic in a dynamic set.',250,0,6,null,0
exec SP_MID_SET_TEXT 110023,'Invalid Dynamic Attribute Filter',250,0,6,null,0
/* End TT#1517-MD -jsobek -Store Service Optimization */
/* Begin TT#1900-MD -JSmith - Versioning Test- Static Attribute able to save a set with a BLANK name */
exec SP_MID_SET_TEXT 110024,'Blank set names are not permitted.',250,0,6,null,0
/* End TT#1900-MD -JSmith - Versioning Test- Static Attribute able to save a set with a BLANK name */

/* Filter Messages 111000-111999 */

exec SP_MID_SET_TEXT 111000,'A Task List with the name you specified already exists.  Please specify a different name.',250,0,6,null,0
exec SP_MID_SET_TEXT 111001,'A Job with the name you specified already exists.  Please specify a different name.',250,0,6,null,0
exec SP_MID_SET_TEXT 111002,'Task List Name must be entered.',250,0,6,null,0
exec SP_MID_SET_TEXT 111003,'Job Name must be entered.',250,0,6,null,0
exec SP_MID_SET_TEXT 111004,'Schedule Name must be entered.',250,0,6,null,0
exec SP_MID_SET_TEXT 111005,'A shortcut for this item already exists.',250,0,6,null,0
exec SP_MID_SET_TEXT 111006,'At least one method is required per merchandise.',250,0,5,null,0
exec SP_MID_SET_TEXT 111007,'At least one merchandise level is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111008,'Current row is not complete.',250,0,5,null,0
exec SP_MID_SET_TEXT 111009,'Input Directory is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111010,'File Suffix is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111011,'Scheduler is not available.',250,0,6,null,0
exec SP_MID_SET_TEXT 111012,'A Schedule with the name you specified already exists.  Please specify a different name.',250,0,5,null,0
exec SP_MID_SET_TEXT 111013,'Repeat Type not defined.',250,0,5,null,0
exec SP_MID_SET_TEXT 111014,'Days must be greater than zero.',250,0,5,null,0
exec SP_MID_SET_TEXT 111015,'Weeks must be greater than zero.',250,0,5,null,0
exec SP_MID_SET_TEXT 111016,'At least one day of the week must be selected.',250,0,5,null,0
exec SP_MID_SET_TEXT 111017,'Months must be greater than zero.',250,0,5,null,0
exec SP_MID_SET_TEXT 111018,'Month Type must be defined.',250,0,5,null,0
exec SP_MID_SET_TEXT 111019,'Hours or Minutes required for duration.',250,0,5,null,0
exec SP_MID_SET_TEXT 111020,'Condition Directory is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111021,'Condition Suffix is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111022,'Run Until Suffix is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111023,'Condition must be specified if "Terminate Repeat After Condition Met" is specified.',250,0,5,null,0
exec SP_MID_SET_TEXT 111024,'Scheduler Thread received an exception.  Attempting to restart.',250,0,5,null,0
exec SP_MID_SET_TEXT 111025,'Scheduler Thread received an exception.  Processing terminated.',250,0,5,null,0
exec SP_MID_SET_TEXT 111026,'Schedule is in an invalid execution state for requested action.',250,0,5,null,0
exec SP_MID_SET_TEXT 111027,'Header Type is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111028,'At least one workflow is required per merchandise.',250,0,5,null,0
exec SP_MID_SET_TEXT 111029,'Rollup Date is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111030,'From Level is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111031,'Executable not found: {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 111032,'ScheduleScanInterval must be greater than CheckForTerminateInterval',250,0,6,null,0
exec SP_MID_SET_TEXT 111033,'Invalid characters found in suffix: wildcards are not allowed',250,0,5,null,0
exec SP_MID_SET_TEXT 111034,'Cooresponding data file not found.  Flag Suffix file: {0}, Data file: {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 111035,'Suffix is invalid: {0}',250,0,5,null,0
exec SP_MID_SET_TEXT 111036,'TaskList has been submitted.',250,0,4,null,0
exec SP_MID_SET_TEXT 111037,'Job has been submitted.',250,0,4,null,0
exec SP_MID_SET_TEXT 111038,'Allocation Tasklist not found for tasklist RID {0}',250,0,5,null,0
exec SP_MID_SET_TEXT 111039,'The selected Job is running.  Are you sure you want to cancel?',250,0,5,null,0
exec SP_MID_SET_TEXT 111040,'The selected Job does not exist.',250,0,5,null,0
exec SP_MID_SET_TEXT 111041,'TaskList is scheduled.  Delete and remove schedule?',250,0,5,null,0
exec SP_MID_SET_TEXT 111042,'Program Path is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111043,'Condition Suffix is too long.  Must be less than 50 characters.',250,0,5,null,0
exec SP_MID_SET_TEXT 111044,'File Suffix is too long.  Must be less than 50 characters.',250,0,5,null,0
exec SP_MID_SET_TEXT 111045,'Run Until Suffix is too long.  Must be less than 50 characters.',250,0,5,null,0
exec SP_MID_SET_TEXT 111046,'Input Directory is too long.  Must be less than 250 characters.',250,0,5,null,0
exec SP_MID_SET_TEXT 111047,'Condition Directory is too long.  Must be less than 250 characters.',250,0,5,null,0
/* BEGIN MID Track #2395 - Auto-suspend tasks */
/* exec SP_MID_SET_TEXT 111048,'TaskList is attached to an schedule and active job.  Wait until job is finished, or hold/cancel job.',250,0,5,null,0 */
/* exec SP_MID_SET_TEXT 111049,'Job is scheduled and active.  Wait until job is finished, or hold/cancel job.',250,0,5,null,0 */
exec SP_MID_SET_TEXT 111048,'TaskList is attached to job(s) that are scheduled and active.  Temporarily hold all jobs?',250,0,5,null,0
exec SP_MID_SET_TEXT 111049,'Job is scheduled and active.  Temporarily hold job?',250,0,5,null,0
/* END MID Track #2395 - Auto-suspend tasks */
exec SP_MID_SET_TEXT 111050,'Batch Executable Directory not found.',250,0,6,null,0
/*Begin TT#1481-MD -jsobek -Task List requires Merchandise */
--exec SP_MID_SET_TEXT 111051,'At least one node descriptor is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111051,'Header Filter required.',250,0,5,null,0
/*End TT#1481-MD -jsobek -Task List requires Merchandise */
/*Begin Track #4010 - JScott - Add all forecast methods and workflows to the scheduler. */
/*exec SP_MID_SET_TEXT 111052,'Default to Method',250,0,2,null,0*/
exec SP_MID_SET_TEXT 111052,'Default to Workflow/Method',250,0,2,null,0
/*End Track #4010 - JScott - Add all forecast methods and workflows to the scheduler. */
exec SP_MID_SET_TEXT 111053,'Default to Workflow',250,0,2,null,0
exec SP_MID_SET_TEXT 111054,'To Level is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111055,'At least one TaskList is required.',250,0,5,null,0
exec SP_MID_SET_TEXT 111056,'At least one Task is required.',250,0,5,null,0
/* BEGIN MID Track #2396 - Add confirmation for Schedule Properties */
exec SP_MID_SET_TEXT 111057,'This job will run immediately.  Is this OK?',250,0,5,null,0
/* END MID Track #2396 - Add confirmation for Schedule Properties */
/* BEGIN MID Track #2423 - Restart run cycle after schedule is updated */
exec SP_MID_SET_TEXT 111058,'The current run cycle for this job will be cancelled.  Is this OK?',250,0,5,null,0
exec SP_MID_SET_TEXT 111059,'The current run cycle for this job will be cancelled and restarted immediately.  Is this OK?',250,0,5,null,0
/* END MID Track #2423 - Restart run cycle after schedule is updated */
/* BEGIN MID Track #3048 - Force job to be held before changing schedule */
exec SP_MID_SET_TEXT 111060,'Schedule will be opened read-only.  It can not be changed because the job is{newline}currently running or waiting to run.  Cancel or hold the job to make modifications.',250,0,5,null,0
/* END MID Track #3048 - Force job to be held before changing schedule */
/* Begin Track #4183 - JScott - Add Size Curve Load to scheduler */
exec SP_MID_SET_TEXT 111061,'(NOTE: The Size Curve Group transaction file should be named yourname.grp.xml and the Size Curve transaction file should be named yourname.crv.xml.',250,0,5,null,0
exec SP_MID_SET_TEXT 111062,'  Only one flag file should be created with the name yourname.grp.xml.flg or yourname.crv.xml.flg, where "flg" is the Flag File Suffix entered above.)',250,0,5,null,0
/* End Track #4183 - JScott - Add Size Curve Load to scheduler */
/* Begin Track #4601 - JScott - System User being changed after Run Now */
exec SP_MID_SET_TEXT 111063,'You do not have authority to save this TaskList.  Do you wish to run original TaskList without changes?',250,0,5,null,0
/* End Track #4601 - JScott - System User being changed after Run Now */
/* Begin Track #4639 - JScott - Invalid delete message from TaskList Explorer */
/* Begin TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job */
/* exec SP_MID_SET_TEXT 111064,'TaskList can not be deleted because it is currently running.',250,0,5,null,0 */
exec SP_MID_SET_TEXT 111064,'TaskList can not be deleted because it is currently running or queued.',250,0,5,null,0
/* End TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job */
/* Begin TT#572 - JScott - Cannot view a running tasklist */
/* exec SP_MID_SET_TEXT 111065,'TaskList can not be updated because it is currently running.',250,0,5,null,0 */
/* Begin TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job */
/* exec SP_MID_SET_TEXT 111065,'TaskList can not be updated because it is currently running.  Would you like to open as read-only?',250,0,5,null,0 */
exec SP_MID_SET_TEXT 111065,'TaskList can not be updated because it is currently running or queued.  Would you like to open as read-only?',250,0,5,null,0
/* End TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job */
/* End TT#572 - JScott - Cannot view a running tasklist */
/* Begin TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job */
/* exec SP_MID_SET_TEXT 111066,'Job can not be deleted because it is currently running.',250,0,5,null,0 */
exec SP_MID_SET_TEXT 111066,'Job can not be deleted because it is currently running or queued.',250,0,5,null,0
/* End TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job */
/* Begin TT#572 - JScott - Cannot view a running tasklist */
/* exec SP_MID_SET_TEXT 111067,'Job can not be updated because it is currently running.',250,0,5,null,0 */
/* Begin TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job */
/* exec SP_MID_SET_TEXT 111067,'Job can not be updated because it is currently running.  Would you like to open as read-only?',250,0,5,null,0 */
exec SP_MID_SET_TEXT 111067,'Job can not be updated because it is currently running or queued.  Would you like to open as read-only?',250,0,5,null,0
/* End TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job */
/* End TT#572 - JScott - Cannot view a running tasklist */
/* End Track #4639 - JScott - Invalid delete message from TaskList Explorer */
/* BEGIN Issue 5117 stodd */
exec SP_MID_SET_TEXT 111068,'A Special Request with the name you specified already exists.  Please specify a different name.',250,0,5,null,0
exec SP_MID_SET_TEXT 111069,'At least one Job is required.',250,0,5,null,0
/* END Issue 5117 stodd */
/* Begin Track #5973 - stodd */
exec SP_MID_SET_TEXT 111070,'Begin tasklist: {0}.',250,0,2,null,0
exec SP_MID_SET_TEXT 111071,'End tasklist: {0}. Elasped time {1}.',250,0,2,null,0
/* End Track #5973 - stodd */
/* Begin TT#2 - stodd - assortment */
exec SP_MID_SET_TEXT 111072,'An assortment with the name you specified already exists.  Please specify a different name.',250,0,2,null,0
/* Begin Track #6423 - JScott - Add Warning when saving task lists */
exec SP_MID_SET_TEXT 111073,'Setting Merchandise, PO, and Header to Default will process all Headers with the specified type and could result in a long runtime.  Are you sure this is correct?',250,0,2,null,0
/* End Track #6423 - JScott - Add Warning when saving task lists */
/* Begin TT#572 - JScott - Cannot view a running tasklist */
exec SP_MID_SET_TEXT 111074,'Open Tasklist as read-only?',250,0,5,null,0
exec SP_MID_SET_TEXT 111075,'Open Job as read-only?',250,0,5,null,0
/* End TT#572 - JScott - Cannot view a running tasklist */
/* End TT#2 - stodd - assortment */
exec SP_MID_SET_TEXT 111076,'Concurrent processes must be set to 1 if File Direction is specified.  Do you want to continue?',250,0,5,null,0  /* TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files */
exec SP_MID_SET_TEXT 111077,'Concurrent processes must be set to 1 if File Direction is specified.  Value has been overridden.',250,0,5,null,0  /* TT#1314-MD - JSmith - Tasklist allows multiple concurrent files with FIFO/FILO processing */

/* Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders */
/* Explorer Messages 112000-112999 */

exec SP_MID_SET_TEXT 112000,'Are you sure you want to delete "{0}"?',250,0,6,null,0
exec SP_MID_SET_TEXT 112001,'Are you sure you want to delete "{0}" and all of its contents?',250,0,6,null,0
exec SP_MID_SET_TEXT 112002,'Are you sure you want to copy "{0}" and all of its contents?',250,0,6,null,0
exec SP_MID_SET_TEXT 112003,'Shared',250,0,6,null,0

/* End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders */

exec SP_MID_SET_TEXT 112004,'Are you sure you want to sign out of the application?',250,0,6,null,0  /* TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner. */
exec SP_MID_SET_TEXT 112005,'No Batch Comp computations defined. Batch Comp has nothing to process.',250,0,6,null,0	/* TT#1595-MD - stodd - batch comp */

exec SP_MID_SET_TEXT 112010,'Store Profile Change Audit Level',250,0,2,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 112011,'Store Attribute Change Audit Level',250,0,2,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 112012,'Store Dynamic Attribute Change Audit Level',250,0,2,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 112013,'Store Characteristic Change Audit Level',250,0,2,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */
exec SP_MID_SET_TEXT 112014,'Header Characteristic Change Audit Level',250,0,2,null,0	/* TT#1517-MD -jsobek -Store Service Optimization */

/* Batch Messages 120000-129999 */

exec SP_MID_SET_TEXT 120000,'Input file: {0}.',250,0,2,null,0
exec SP_MID_SET_TEXT 120001,'CommitLimit not found or invalid.  {0} was used.',250,0,5,null,0
exec SP_MID_SET_TEXT 120002,'BulkInsertBatchSize not found or invalid.  {0} was used.',250,0,5,null,0
exec SP_MID_SET_TEXT 120003,'SerializeXMLFile not found or invalid.  {0} was used.',250,0,5,null,0
exec SP_MID_SET_TEXT 120004,'AllowAutoAdds not found or invalid.  {0} was used.',250,0,5,null,0
exec SP_MID_SET_TEXT 120005,'UseBulkInsert not found or invalid.  {0} was used.',250,0,5,null,0
exec SP_MID_SET_TEXT 120006,'BulkInsertProcessReference is required for bulk insert.',250,0,6,null,0
exec SP_MID_SET_TEXT 120007,'BulkInsertDatabaseReference is required for bulk insert.',250,0,6,null,0
exec SP_MID_SET_TEXT 120008,'Invalid stock type.  History/Plan load cancelled.',250,0,6,null,0
exec SP_MID_SET_TEXT 120009,'BulkInsertRetryCount not found or invalid.  {0} was used.',250,0,5,null,0
exec SP_MID_SET_TEXT 120010,'BulkInsertCommandTimeout not found or invalid.  {0} was used.',250,0,5,null,0
exec SP_MID_SET_TEXT 120011,'RollupValues not found or invalid.  {0} was used.',250,0,5,null,0
exec SP_MID_SET_TEXT 120012,'FromLevel is required',250,0,5,null,0
exec SP_MID_SET_TEXT 120013,'FromLevel {0} is Invalid',250,0,5,null,0
exec SP_MID_SET_TEXT 120014,'ToLevel {0} is Invalid',250,0,5,null,0
exec SP_MID_SET_TEXT 120015,'FromLevel {0} is less than ToLevel {1}',250,0,5,null,0
exec SP_MID_SET_TEXT 120016,'Invalid argument: {0}',250,0,5,null,0
exec SP_MID_SET_TEXT 120017,'Process starting',250,0,5,null,0
exec SP_MID_SET_TEXT 120018,'Process ending',250,0,5,null,0
exec SP_MID_SET_TEXT 120019,'Invalid size code multiple {0} for size {1}',250,0,4,null,0
exec SP_MID_SET_TEXT 120020,'Allocation Header {0} is in use under a multi-header and cannot be modified, deleted, or reset by this process.',250,0,4,null,0
exec SP_MID_SET_TEXT 120021,'Allocation Header {0} is a multi-header. Multi-headers cannot be modified or removed through this process.',250,0,4,null,0
exec SP_MID_SET_TEXT 120022,'Rollup settings not found at {0}. Values will be loaded but not rolled up.',250,0,4,null,0
exec SP_MID_SET_TEXT 120023,'Invalid rollup settings found. Values will be loaded but not rolled up.',250,0,4,null,0
exec SP_MID_SET_TEXT 120024,'Rollup version {0} is not valid.',250,0,4,null,0
exec SP_MID_SET_TEXT 120025,'Invalid Method definition. Tasklist: {0}  Step Seq: {1}',250,0,6,null,0
exec SP_MID_SET_TEXT 120026,'Tasklist step is missing the Workflow/Method definition. Tasklist: {0}  Step Seq: {1}',250,0,6,null,0
exec SP_MID_SET_TEXT 120027,'[RelieveAllStores] field default value as specified in Relieve Intransit App.config: [{0}] ',250,0,2,null,0 /* MID Track 5694 MA enhancement Relieve IT by Header ID */
/* Begin MOD - JScott - Build Pack Criteria Load */
exec SP_MID_SET_TEXT 120028,'Build Pack Config Load input record must contain 6 fields.',250,0,6,null,0
exec SP_MID_SET_TEXT 120029,'Criteria Name (field 1) exceeds length of database column ({0}).',250,0,6,null,0
exec SP_MID_SET_TEXT 120030,'Error converting Component Minimum Quantity (field 2): {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 120031,'Error converting Size Multiple (field 3): {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 120032,'Combo Name (field 4) exceeds length of database column ({0}).',250,0,6,null,0
exec SP_MID_SET_TEXT 120033,'Error converting Pack Multiple (field 5): {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 120034,'Error converting Combo Max Packs (field 6): {0}',250,0,6,null,0
/* End MOD - JScott - Build Pack Criteria Load */
/* Begin TT#710 - JSmith - Generate relieve intransit */
exec SP_MID_SET_TEXT 120035,'Output file location = {0}',250,0,2,null,0
exec SP_MID_SET_TEXT 120036,'Output file name = {0}',250,0,2,null,0
exec SP_MID_SET_TEXT 120037,'Headers per file = {0}',250,0,2,null,0
exec SP_MID_SET_TEXT 120038,'Flag file suffix = {0}',250,0,2,null,0
exec SP_MID_SET_TEXT 120039,'Header Command = {0}',250,0,2,null,0
exec SP_MID_SET_TEXT 120040,'No headers to relieve',250,0,2,null,0
exec SP_MID_SET_TEXT 120041,'Generate relieve all stores for header {0}',250,0,2,null,0
exec SP_MID_SET_TEXT 120042,'Ending Process. Return code = {0}',250,0,2,null,0
/* End TT#710 */
/* Begin TT#767 - JSmith - Purge Performance */
exec SP_MID_SET_TEXT 120043,'Starting purge process',250,0,2,null,0
exec SP_MID_SET_TEXT 120044,'Determining purge dates',250,0,2,null,0
exec SP_MID_SET_TEXT 120045,'Completed determining purge dates',250,0,2,null,0
exec SP_MID_SET_TEXT 120046,'Removing history, forecasts and external intransit',250,0,2,null,0
exec SP_MID_SET_TEXT 120047,'Completed removing history, forecasts and external intransit',250,0,2,null,0
exec SP_MID_SET_TEXT 120048,'Removing released and shipped headers',250,0,2,null,0
exec SP_MID_SET_TEXT 120049,'Completed removing released and shipped headers',250,0,2,null,0
exec SP_MID_SET_TEXT 120050,'Removing audit information',250,0,2,null,0
exec SP_MID_SET_TEXT 120051,'Completed removing audit information',250,0,2,null,0
exec SP_MID_SET_TEXT 120052,'Removing completed schedules',250,0,2,null,0
exec SP_MID_SET_TEXT 120053,'Completed removing completed schedules',250,0,2,null,0
exec SP_MID_SET_TEXT 120054,'Removing deleted users',250,0,2,null,0
exec SP_MID_SET_TEXT 120055,'Completed removing deleted users',250,0,2,null,0
exec SP_MID_SET_TEXT 120056,'Removing deleted groups',250,0,2,null,0
exec SP_MID_SET_TEXT 120057,'Completed removing deleted groups',250,0,2,null,0
exec SP_MID_SET_TEXT 120058,'Purge Complete',250,0,2,null,0
/* End TT#767 */
/*Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2 */
exec SP_MID_SET_TEXT 120059,'Chain Set Percent Config Load input record must contain 5 fields.',250,0,6,null,0
exec SP_MID_SET_TEXT 120060,'Error converting Current Plan Week (field 1): {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 120061,'Error converting Merchandise ID (field 2): {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 120062,'Error converting Store Attribute (field 3): {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 120063,'Error converting Store Attribute Set (field 4): {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 120064,'Cannot Insert Null or 0 into Percentage (field 5): {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 120065,'Could not Find Merchandise ID (field 2): {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 120066,'Could not Find Store Attribute (field 3): {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 120067,'Could not Find Store Attribute Set (field 4): {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 120068,'Could not Find Plan Week (field 0): {0}',250,0,6,null,0
exec SP_MID_SET_TEXT 120069,' For week {0} and Store Group {1} and Variable, {2}, all plan values are zeros and unlocked--no balancing needed.',30,10,null,null,50
exec SP_MID_SET_TEXT 120070,'Error determining purge date. Review messages in Hierarchy Service audit.',250,0,6,null,0  /* Begin TT#2198 - JSmith - Headers not purging */
exec SP_MID_SET_TEXT 120071,'Daily Percentages Config Load input record must contain 5 fields.',250,0,6,null,0 /* TT#43 – MD – DOConnell – Projected Sales Enhancement */
exec SP_MID_SET_TEXT 120072,'Removing empty dynamic store sets',250,0,2,null,0				/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 120073,'Completed removing empty dynamic store sets',250,0,2,null,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 120074,'Purge was unable to delete one or more empty dynamic store sets because they were in use. Check the store service audit for a list of sets.',250,0,2,null,0	/* TT#739-MD - STodd - delete stores */

/* BEGIN TT#3265 - DOConnell - performance issue with Store Eligibility Load */
exec SP_MID_SET_TEXT 120075, 'Invalid Eligibility Modifier Model {0} ',250,0,6,null,0
exec SP_MID_SET_TEXT 120076, 'Invalid FWOS Modifier Model {0} ',250,0,6,null,0
exec SP_MID_SET_TEXT 120077, 'Invalid Stock Modifier Model {0} ',250,0,6,null,0
exec SP_MID_SET_TEXT 120078, 'Invalid Sales Modifier Model {0} ',250,0,6,null,0
exec SP_MID_SET_TEXT 120079, 'Invalid Min Plus Sales Value {0} ',250,0,6,null,0
exec SP_MID_SET_TEXT 120080, 'Invalid Eligibility Modifier Value {0} ',250,0,6,null,0
exec SP_MID_SET_TEXT 120081, 'Invalid FWOS Modifier Value {0} ',250,0,6,null,0
exec SP_MID_SET_TEXT 120082, 'Invalid Stock Modifier Value {0} ',250,0,6,null,0
exec SP_MID_SET_TEXT 120083, 'Invalid Sales Modifier Value {0} ',250,0,6,null,0
exec SP_MID_SET_TEXT 120084, 'Cannot have both {0} Value and Model ',250,0,6,null,0
exec SP_MID_SET_TEXT 120085, 'Invalid Min Ship Qty value',250,0,6,null,0
exec SP_MID_SET_TEXT 120086, 'Invalid VSW Store',250,0,6,null,0
exec SP_MID_SET_TEXT 120087, 'Invalid Pack Threshold value',250,0,6,null,0
exec SP_MID_SET_TEXT 120088, 'Invalid Item Max value',250,0,6,null,0
exec SP_MID_SET_TEXT 120089, 'Invalid FWOS Max value',250,0,6,null,0
exec SP_MID_SET_TEXT 120090, 'Invalid FWOS Max model',250,0,6,null,0
exec SP_MID_SET_TEXT 120091, 'Invalid Push To Backstock value',250,0,6,null,0
exec SP_MID_SET_TEXT 120092, 'Invalid Store Eligibility Type {0}',250,0,6,null,0
/* END TT#3265 - DOConnell - performance issue with Store Eligibility Load */

/* Begin TT#1581-MD - stodd - Header Reconcile API	*/
exec SP_MID_SET_TEXT 120093,'Allocation Header Transaction # {0} has multiple header matches. Cannot determine Header ID.',250,0,6,null,0
exec SP_MID_SET_TEXT 120094,'Allocation Header Transaction # {0} has more than one color on Pack. Cannot determine Header ID.',250,0,6,null,0
exec SP_MID_SET_TEXT 120095,'Allocation Header Transaction # {0} has more than one color. Cannot determine Header ID.',250,0,6,null,0
exec SP_MID_SET_TEXT 120096,'Allocation Header Transaction # {0} contains an invalid header type of "{1}". Cannot determine Header ID.',250,0,6,null,0
exec SP_MID_SET_TEXT 120097,'Allocation Header Transaction # {0} is missing StyleId. Cannot determine Header ID.',250,0,6,null,0
exec SP_MID_SET_TEXT 120098,'Allocation Header Transaction # {0} is missing DistCenter. Cannot determine Header ID.',250,0,6,null,0
exec SP_MID_SET_TEXT 120099,'Allocation Header Transaction # {0} is missing PurchaseOrder. Cannot determine Header ID.',250,0,6,null,0
exec SP_MID_SET_TEXT 120100,'Allocation Header Transaction # {0} has no header matches. Cannot determine Header ID.',250,0,6,null,0
exec SP_MID_SET_TEXT 120101,'Allocation Header Transaction # {0} contains HeaderId, {1}, and will be processed using that HeaderId.',250,0,2,null,0
exec SP_MID_SET_TEXT 120102,'Allocation Header Transaction # {0} HeaderId {1} Reset successful. Continuing with Removal.',250,0,2,null,0
exec SP_MID_SET_TEXT 120103,'Allocation Header Transaction # {0} HeaderId {1} Reset failed. Header will not be removed.',250,0,6,null,0
exec SP_MID_SET_TEXT 120104,'Allocation Header Transaction # {0} Invalid header action of {1}. Cannot determine Header ID.',250,0,6,null,0
exec SP_MID_SET_TEXT 120105,'Allocation Header Transaction # {0} found two header matches; one released and one not released. The unreleased header, {1}, was removed. The released header, {2}, was kept.',250,0,5,null,0
/* End TT#1581-MD - stodd - Header Reconcile API */
/* TT#3655 - doconnell – Uploading Sales Modifier Value breaks inheritance for other Models*/
exec SP_MID_SET_TEXT 120106, 'Either Item Max or FWOS Max is required when loading Min Ship Qty or % Pack Threshold value ',250,0,6,null,0
/* TT#3661 - DOConnell - Eligibility Upload - Similar Store Date Range */
exec SP_MID_SET_TEXT 120107, '{0} required ',250,0,6,null,0
/* TT#541 – MD - DOConnell - OTS Forecast receive a "nothing to spread to exception" when weeks are locked */
exec SP_MID_SET_TEXT 120108, 'There were no available cells to spread {0} store value to % contribution for week {1}. ',250,0,5,null,0
/*Begin TT#3630 - JSmith - Delete My Hierarchy */
exec SP_MID_SET_TEXT 120109,'Removing deleted hierarchy nodes',250,0,2,null,0
exec SP_MID_SET_TEXT 120110,'Completed removing deleted hierarchy nodes',250,0,2,null,0
exec SP_MID_SET_TEXT 120111,'{0} has been successfully deleted',250,0,2,null,0
/*End TT#3630 - JSmith - Delete My Hierarchy */
/*Begin TT#3822 - JSmith - Add Stop Time to Purge */
exec SP_MID_SET_TEXT 120112,'Cutoff time exceeded.  Process is stopping.',250,0,5,null,0
exec SP_MID_SET_TEXT 120113,'Cutoff time set for {0}.',250,0,2,null,0
exec SP_MID_SET_TEXT 120114,'Cutoff time {0} is invalid and will be ignored.',250,0,2,null,0
/*End TT#3822 - JSmith - Add Stop Time to Purge */
exec SP_MID_SET_TEXT 120115,'Similar store {0} is either undefined or inactive for merchandise {1} and store {2}',250,0,2,null,0	/* TT#1580-MD - stodd - Data Layer Request -  new message for invalid similar store */

/* Begin TT#1581-MD - stodd - Header Reconcile API */
exec SP_MID_SET_TEXT 120116,'Allocation Header Transaction # {0} is missing {1}. Cannot determine Header ID.',250,0,6,null,0
exec SP_MID_SET_TEXT 120117,'Allocation Header Transaction # {0} is missing the header characteristic: {1}. Cannot determine Header ID.',250,0,6,null,0
exec SP_MID_SET_TEXT 120118,'No Keys To Match headers on were found in file at: {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 120119,'Keys To Match headers on have not been defined in file at: {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 120120,'No Header ID keys were found in file at: {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 120121,'Header ID keys have not been defined in file at: {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 120122,'No {0} specified.',250,0,6,null,0
exec SP_MID_SET_TEXT 120123,'Input directory does not exist or is not specified correctly: {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 120124,'Output directory does not exist or is not specified correctly: {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 120125,'Header Processing Keys File does not exist or is not specified correctly: {0}.',250,0,6,null,0
/* End TT#1581-MD - stodd - Header Reconcile API */

/* Begin TT#1386-MD - stodd - Scheduler Job Manager API */
exec SP_MID_SET_TEXT 120126,'User Name does not exist in application. User Name = {0}.',250,0,6,null,0
exec SP_MID_SET_TEXT 120127,'Command: SchedulerJobManager {0}  Successfully Completed.',250,0,2,null,0
exec SP_MID_SET_TEXT 120128,'Matching jobs table is null. Schedule Name: {0} Job Name: {1}  User RID: {2}',250,0,6,null,0
exec SP_MID_SET_TEXT 120129,'No Matching jobs were found. Schedule Name: {0} Job Name: {1}  User RID: {2}',250,0,5,null,0
/* End TT#1386-MD - stodd - Scheduler Job Manager API */
/* Begin TT#5048 - JSmith - Allows header type to be changed when group allocation */
exec SP_MID_SET_TEXT 120130,'Cannot process Allocation Header {0} when the type is {1}.',250,0,4,null,0
/* End TT#5048 - JSmith - Allows header type to be changed when group allocation */

/* Security Messages 130000-139999 */

exec SP_MID_SET_TEXT 130000,'Password confirmation failed.',250,0,6,null,0
exec SP_MID_SET_TEXT 130001,'{0} already exists in this folder.',250,0,6,null,0
exec SP_MID_SET_TEXT 130002,'Please drop on a Merchandise Folder.',250,0,6,null,0
exec SP_MID_SET_TEXT 130003,'Cannot assign security at the color or size level.',250,0,6,null,0
exec SP_MID_SET_TEXT 130004,'Cannot assign personal items to other users.',250,0,6,null,0
exec SP_MID_SET_TEXT 130005,'You are not authorized to plan this ',250,0,6,null,0
/* Begin Track #4815 - JSmith - #283-User (Security) Maintenance */
exec SP_MID_SET_TEXT 130006,'This will be deleted when Purge runs.',250,0,2,null,0
exec SP_MID_SET_TEXT 130007,'User {0} can not be deleted because item(s) owned by user are currently running.',250,0,5,null,0
exec SP_MID_SET_TEXT 130008,'Items for user {0} will be moved to user {1}.  This action is not reversible.  Do you want to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 130009,'All items for user {0} have been assigned to user {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 130010,'All items for user {0} have been moved to user {1}.',250,0,2,null,0
exec SP_MID_SET_TEXT 130011,'User {0} is assigned to another user.  Deleting the user will delete assigned items.  Do you want to continue?',250,0,5,null,0
exec SP_MID_SET_TEXT 130012,'Do you want to activate user {0}?',250,0,2,null,0
exec SP_MID_SET_TEXT 130013,'You are not authorized to plan one or more of the low level merchandise and/or versions.',250,0,6,null,0
/* End Track #4815 */
/* Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer */
exec SP_MID_SET_TEXT 130014,'The User is logged on.  Assignment cannot be performed until they are logged off.',250,0,6,null,0
/* End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer */
/* Begin TT#2 - RMatelic - Assortment Planning */
exec SP_MID_SET_TEXT 130015,'You are not authorized for this function',250,0,5,null,0	
/* End TT#2 */

/* Size Method Messages 140000-149999 */
exec SP_MID_SET_TEXT 140000,'Color must be selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 140001,'Size must be selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 140002,'Size dimension must be selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 140003,'Required when Fill Zero Indicator is selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 140004,'Value should only be set when Fill Zero Indicator is selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 140005,'Size quantity is required when the Fill Up To Rule is selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 140006,'Size quantity should only be entered when Fill Up To Rule is selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 140007,'Size quantity is required when the Quantity Rule is selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 140008,'Size quantity should only be entered when Quantity Rule is selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 140009,'Size quantity should not be entered when no rule is selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 140010,'Data type cannot be dropped here.',250,0,4,null,0
exec SP_MID_SET_TEXT 140011,'Method can only accept one Basis Header.',250,0,4,null,0
exec SP_MID_SET_TEXT 140012,'Selected header does not contain bulk colors.',250,0,4,null,0
exec SP_MID_SET_TEXT 140013,'Selection of a basis header is required.',250,0,4,null,0
exec SP_MID_SET_TEXT 140014,'Please specify units or percent.',250,0,4,null,0
exec SP_MID_SET_TEXT 140015,'Size curve is required when a size group is selected.',250,0,4,null,0
exec SP_MID_SET_TEXT 140016,'Size group is required.',250,0,4,null,0
exec SP_MID_SET_TEXT 140017,'Condition is required to create a filter.',250,0,4,null,0
exec SP_MID_SET_TEXT 140018,'Value is required to create a filter.',250,0,4,null,0
exec SP_MID_SET_TEXT 140019,'Operator is required to create a complex filter.',250,0,4,null,0
exec SP_MID_SET_TEXT 140020,'Curve percentages must sum to 100.',250,0,5,null,0
/* Begin TT#433 - JScott - Size Curve Node Properties after running the Task list it fails with a Warning.  Audit needs message to identify which size curves do not generate based on no sales data existing. */
/* Begin TT#155 - JScott - Size Curve Method */
/* exec SP_MID_SET_TEXT 140021,'No size data exists for any store.  Size Curve has not been created.',250,0,5,null,0 */
exec SP_MID_SET_TEXT 140021,'No size data exists for any store.  Size Curve "{0}" has not been created.',250,0,5,null,0
/* End TT#155 - JScott - Size Curve Method */
/* End TT#433 - JScott - Size Curve Node Properties after running the Task list it fails with a Warning.  Audit needs message to identify which size curves do not generate based on no sales data existing. */
/* Begin TT#644 - JScott - Unable to allocate using Size Need with size curves at the Class level. */
exec SP_MID_SET_TEXT 140022,'Size Curve "{0}" is in use and cannot be updated.',250,0,5,null,0
/* End TT#644 - JScott - Unable to allocate using Size Need with size curves at the Class level. */
/* Begin TT#728 - APicchetti - Soft text additions for build packs */
/* Begin TT#1350 - JSmith - Missing text */
exec SP_MID_SET_TEXT 140030,'A header is selected to use with this method.\n Do you want to override the size data with a size curve?' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140031,'A header is selected to use with this method.\n Do you want to override the size data with a size group?' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140032,'Apply Sizes from Header?' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140033,'Are you sure that you would like delete this pack combination?' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140034,'Are you sure that you would like delete this pack?' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140035,'Do you want to apply the size information to the pack combinations?' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140037,'Sizes are selected to use with this method. \n Do you want to override the size data with a size curve?' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140038,'Sizes are selected to use with this method. \n Do you want to override the size data with a size group?' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140039,'The chosen pack option was successfully applied.' ,250,0,2,null,0
exec SP_MID_SET_TEXT 140040,'The interactive mode of the Build Packs method \n only supports one header at a time. Only the first \n chosen header will be processed.' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140041,'There are no search results to show.' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140042,'You cannot add packs to a vendor defined pack combination.' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140043,'You cannot delete a vendor defined pack combination.' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140044, 'Apply' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140045, '&Add Pack Combination' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140046, 'Add &Pack' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140047, '&Delete' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140048, '%' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140049, 'Units' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140050, '%' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140051, 'Units' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140052, '%' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140053, 'Units' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140054, 'Method' ,250,0,5,null,0
exec SP_MID_SET_TEXT 140055, 'Property Names' ,250,0,5,null,0
/* End TT#1350 */
exec SP_MID_SET_TEXT 140056, 'You must enter a name for the method before you can save or process.' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140057, 'You must enter a description for the method before you can save or process.' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140058, 'All of your defined packs must have pack quantity and maximum packs values defined.' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140059, 'You must have at least one pack combination chosen to save or process.' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140060, '{0} on the constraints tab cannot be blank.' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140061, '{0} cannot be a negative number.' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140062, '{0} must be a whole number, no decimal places are accepted.' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140063, 'When {0} is greater than 0, then at least one of the Variance Options below must be checked' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140064, '{0} cannot be a negative number or greater than 100%.' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140065, 'The Reserve As Bulk and Reserve As Packs must add up to 100%.' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140066, '{0} and {1} must add up to the value in {2}' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140067, 'The Build Packs interactive method will only handle Zero or One header selection. Multiple headers are selected' ,250,0,4,null,0
/* End TT#728 - APicchetti - Soft text additions for build packs */
--Begin TT#1076 - JScott - Size Curves by Set
exec SP_MID_SET_TEXT 140068, 'Attribute Set required when using Size Curve By Set' ,250,0,4,null,0
--End TT#1076 - JScott - Size Curves by Set
--Begin TT#2168 - DOConnell - Size Curve Method 20% min 50% max Issue
exec SP_MID_SET_TEXT 140069, 'Size Curve: {0} - All Sizes locked for store: {1} due to Min/Max constraints  Default curve used' ,250,0,4,null,0
--End TT#2168 - DOConnell - Size Curve Method 20% min 50% max Issue
--Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
exec SP_MID_SET_TEXT 140070, 'Invalid date' ,250,0,4,null,0
--Begin TT#280 - MD - DOConnell - Daily Percentages error messages are incorrect
exec SP_MID_SET_TEXT 140071, 'Daily Percentages adjusted to equal 100' ,250,0,2,null,0 
exec SP_MID_SET_TEXT 140072, 'Invalid daily percentage' ,250,0,4,null,0
exec SP_MID_SET_TEXT 140073, 'Default Percentages adjusted to equal 100' ,250,0,2,null,0 
exec SP_MID_SET_TEXT 140074, 'Header enqueue lost for header: {0}. The most likely cause is that a ''Release Resources'' has been processed.' ,250,0,5,null,0 
--End TT#280 - MD - DOConnell - Daily Percentages error messages are incorrect
--End TT#43 - MD - DOConnell - Projected Sales Enhancement
/* Begin TT#1019 - MD - stodd - prohibit allocation actions against GA */
exec SP_MID_SET_TEXT 140075, 'Actions - DISABLED because a header(s) belongs to a Group Allocation.' ,250,0,2,null,0 
exec SP_MID_SET_TEXT 140076, 'Process Action - DISABLED because a header(s) belongs to a Group Allocation.' ,250,0,2,null,0 
/* End TT#1019 - MD - stodd - prohibit allocation actions against GA */


/* Custom User Exceptions 199000 - 199999 */
/* NOTE -- DO NOT LIST ANY ADDITIONAL USER EXCEPTIONS HERE. THEY SHOULD BE ADDED IN THE COMPUTATIONS. */
exec SP_MID_SET_TEXT 199000,'User Error encountered: {0}',250,0,5,null,0

/* Form Names 200000-209999 */

exec SP_MID_SET_TEXT 200000,'Merchandise Explorer',50,0,null,null,0
exec SP_MID_SET_TEXT 200001,'Store Explorer',50,0,null,null,0
exec SP_MID_SET_TEXT 200002,'Workspace Explorer',50,0,null,null,0
exec SP_MID_SET_TEXT 200003,'Workflow/Method Explorer',50,0,null,null,0
exec SP_MID_SET_TEXT 200004,'Store Filter Explorer',50,0,null,null,0
exec SP_MID_SET_TEXT 200010,'Hierarchy Properties',50,0,null,null,0
exec SP_MID_SET_TEXT 200011,'Node Properties',50,0,null,null,0
exec SP_MID_SET_TEXT 200012,'Eligibility Model',50,0,null,null,0
exec SP_MID_SET_TEXT 200013,'Sales Modifier Model',50,0,null,null,0
exec SP_MID_SET_TEXT 200014,'Stock Modifier Model',50,0,null,null,0
exec SP_MID_SET_TEXT 200015,'General Allocation Method',50,0,null,null,0
exec SP_MID_SET_TEXT 200016,'Rule Method',50,0,null,null,0
exec SP_MID_SET_TEXT 200017,'OTS Forecast Method',50,0,null,null,0
exec SP_MID_SET_TEXT 200018,'Fill Sizes Method',50,0,null,null,0
exec SP_MID_SET_TEXT 200019,'Override Method',50,0,null,null,0
exec SP_MID_SET_TEXT 200020,'Velocity Method',50,0,null,null,0
exec SP_MID_SET_TEXT 200021,'Allocation Workflow',50,0,null,null,0
exec SP_MID_SET_TEXT 200022,'Forecast Workflow',50,0,null,null,0
exec SP_MID_SET_TEXT 200023,'Allocation Scheduler',50,0,null,null,0
exec SP_MID_SET_TEXT 200024,'Similar Stores',50,0,null,null,0
exec SP_MID_SET_TEXT 200025,'Global Options',50,0,null,null,0
exec SP_MID_SET_TEXT 200026,'Calendar Model List',50,0,null,null,0
exec SP_MID_SET_TEXT 200027,'Calendar Model Definition',50,0,null,null,0
exec SP_MID_SET_TEXT 200028,'Calendar Display',50,0,null,null,0
exec SP_MID_SET_TEXT 200029,'53rd Week Selector',50,0,null,null,0
exec SP_MID_SET_TEXT 200030,'Administration',50,0,null,null,0
exec SP_MID_SET_TEXT 200031,'Audit',50,0,null,null,0
exec SP_MID_SET_TEXT 200032,'OTS Forecast Selection',50,0,null,null,0
exec SP_MID_SET_TEXT 200033,'OTS Forecast Review',50,0,null,null,0
exec SP_MID_SET_TEXT 200034,'Style Review',50,0,null,null,0
exec SP_MID_SET_TEXT 200035,'Summary Review',50,0,null,null,0
exec SP_MID_SET_TEXT 200036,'Size Review',50,0,null,null,0
exec SP_MID_SET_TEXT 200037,'Store Characteristics',50,0,null,null,0
exec SP_MID_SET_TEXT 200038,'Store Profiles',50,0,null,null,0
exec SP_MID_SET_TEXT 200039,'Store Attributes',50,0,null,null,0
exec SP_MID_SET_TEXT 200040,'Header Characteristics',50,0,null,null,0
exec SP_MID_SET_TEXT 200041,'Text Editor',50,0,null,null,0
exec SP_MID_SET_TEXT 200042,'Basis Size Method', 50, 0, 5, NULL, 0
exec SP_MID_SET_TEXT 200043,'Size Override Method', 50, 0, 5, NULL, 0
exec SP_MID_SET_TEXT 200044,'Reserve Size Method', 50, 0, 5, NULL, 0
exec SP_MID_SET_TEXT 200045,'Size Need Method', 50, 0, 5, NULL, 0
exec SP_MID_SET_TEXT 200046,'Allocation Workspace Filter', 50, 0, 5, NULL, 0
exec SP_MID_SET_TEXT 200047,'Security Inheritance', 50, 0, 5, NULL, 0
exec SP_MID_SET_TEXT 200048,'Size Constraint Model Maintenance', 50, 0, 5, NULL, 0
/* exec SP_MID_SET_TEXT 200049,'Size Fringe Model Maintenance',50,0,5,NULL,0  */   /* MID Track 3619 Remove Fringe */
exec SP_MID_SET_TEXT 200050,'Size Alternative Model Maintenance',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200051,'Override Low Level Versions',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200052,'Matrix Balance',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200053,'Size Code Maintenance',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200054,'Matrix Balance',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200055,'About Logility - RO',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200056,'Forecast Model',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200057,'Forecast Balance Model',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200058,'Copy Chain Forecast',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200059,'Copy Store Forecast',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200060,'Forecast Spread',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200061,'General Assortment Method',50,0,5,NULL,0
/*Begin Assortment Planning - JScott - Assortment Planning Changes */
exec SP_MID_SET_TEXT 200062,'Assortment Review',50,0,5,NULL,0
/*End Assortment Planning - JScott - Assortment Planning Changes */
exec SP_MID_SET_TEXT 200063,'Allocation View Selection',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200064,'Color Browser',50,0,5,NULL,0
/* Begin Product Characteristics - JSmith */
exec SP_MID_SET_TEXT 200065,'Product Characteristics',50,0,null,null,0
exec SP_MID_SET_TEXT 200066,'Properties',50,0,null,null,0
exec SP_MID_SET_TEXT 200067,'Export',50,0,null,null,0
/* End Product Characteristics */
exec SP_MID_SET_TEXT 200068,'Modify Sales',50,0,5,NULL,0
exec SP_MID_SET_TEXT 200069,'FWOS Modifier Model',50,0,null,null,0
exec SP_MID_SET_TEXT 200070,'User Options',50,0,null,null,0
exec SP_MID_SET_TEXT 200071,'Exclude Low Levels',50,0,null,null,0
/* Begin Enhancement - JScott - Export Method - Part 2 */
exec SP_MID_SET_TEXT 200072,'Export Method',50,0,null,null,0
/* End Enhancement - JScott - Export Method - Part 2 */
exec SP_MID_SET_TEXT 200073,'Store Size Curves',50,0,null,null,0		/* BEGIN MID Track 4970 - add locking */ 
exec SP_MID_SET_TEXT 200074,'Size Groups',50,0,null,null,0				/* END MID Track 4970 */ 
/* Begin Track #4815 - JSmith - #283-User (Security) Maintenance */
exec SP_MID_SET_TEXT 200075,'Assign User',50,0,null,null,0
/* End Track #4815 */
/* Begin Enhancement #5176 - JSmith - Audit Filter  */
exec SP_MID_SET_TEXT 200076,'Audit Filter',50,0,null,null,0
/* End Track #5176 */
/* Begin Enhancement #5004 - KJohnson - Global Unlock */
exec SP_MID_SET_TEXT 200077,'Global Unlock',50,0,null,null,0
/* End Enhancement #5004 - KJohnson - Global Unlock */
/* Begin Enhancement #5196 - KJohnson - Rollup  */
exec SP_MID_SET_TEXT 200079,'Rollup',50,0,null,null,0
/* End Enhancement #5196 - KJohnson - Rollup */
/* Begin TT#2 - Ron Matelic - Assortment Planning */
exec SP_MID_SET_TEXT 200080,'Assortment Workspace',50,0,null,null,0	
exec SP_MID_SET_TEXT 200081,'Assortment Workspace Filter',50,0,null,null,0
/* End TT#2 */
/* Begin TT#155 - JSmith - Size Curve Method */
exec SP_MID_SET_TEXT 200082,'Size Curve Method', 50, 0, 5, NULL, 0
/* End TT#155 */
exec SP_MID_SET_TEXT 200083,'Node Properties Overrides Report', 50, 0, 5, null, 0	/* TT#274 - RMatelic - Unrelataed to specific issue */ 
exec SP_MID_SET_TEXT 200084,'Build Packs Method',50,0,5, null,0 /* TT#370 */
exec SP_MID_SET_TEXT 200085,'Global Lock',50,0,null,null,0 /* TT#43 – MD – DOConnell – Projected Sales Enhancement */
/* Begin TT#155 - JSmith - Size Curve Method */
exec SP_MID_SET_TEXT 200082,'Size Curve Method', 50, 0, 5, NULL, 0
/* End TT#155 */
exec SP_MID_SET_TEXT 200083,'Node Properties Overrides Report', 50, 0, 5, null, 0	/* TT#274 - RMatelic - Unrelataed to specific issue */ 
exec SP_MID_SET_TEXT 200084,'Build Packs Method',50,0,5, null,0 /* TT#370 */
exec SP_MID_SET_TEXT 200086,'FWOS Max Model',50,0, null,null,0 /* TT#102 – MD – DOConnell – FWOS Max Model enhancement */
exec SP_MID_SET_TEXT 200087,'In Use',50,0, null,null,0		   /* TT#110–MD – RMatelic – In Use Tool */
exec SP_MID_SET_TEXT 200088,'Processing Direction',50,0, null,null,0		   /* TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files */
exec SP_MID_SET_TEXT 200089,'Group Allocation Method',50,0, null,null,0		/* TT#708-MD - Stodd - Group Allocation Prototype.   */
exec SP_MID_SET_TEXT 200090,'Group Allocation',50,0, null,null,0			/* TT#488-MD - Stodd - Group Allocation.   */
exec SP_MID_SET_TEXT 200091,'DC Carton Rounding Method',50,0, null,null,0   /* TT#1652-MD - RMatelic - DC Carton rounding */
exec SP_MID_SET_TEXT 200092,'Create Master Headers Method',50,0, null,null,0   /* TT#1966-MD - JSmith - DC Fulfillment */
exec SP_MID_SET_TEXT 200093,'DC Fulfillment Method',50,0, null,null,0   /* TT#1966-MD - JSmith - DC Fulfillment */
exec SP_MID_SET_TEXT 200094,'Planning Extract',50,0,null,null,0   /* TT#2131-MD - JSmith - Halo Integration */

/* Menu Items */
exec SP_MID_SET_TEXT 300000,'&File',50,1,12,3,0
exec SP_MID_SET_TEXT 300010,'&Close',50,1,12,3,0
exec SP_MID_SET_TEXT 300020,'&Save',50,1,12,3,0
exec SP_MID_SET_TEXT 300030,'Save &As',50,1,12,3,0
exec SP_MID_SET_TEXT 300040,'&Export',50,1,12,3,0
exec SP_MID_SET_TEXT 300045,'Login As Administrator',50,1,12,3,0 /* TT#1521-MD -jsobek -Active Directory Authentication */
exec SP_MID_SET_TEXT 300050,'E&xit',50,1,12,3,0
exec SP_MID_SET_TEXT 300100,'&Edit',50,1,12,3,0
/* Begin Track #4815 - JSmith - #283-User (Security) Maintenance */
/* exec SP_MID_SET_TEXT 300110,'Cu&t',50,1,12,3,0 */
/* exec SP_MID_SET_TEXT 300120,'&Copy',50,1,12,3,0 */
/* exec SP_MID_SET_TEXT 300130,'&Paste',50,1,12,3,0 */
/* exec SP_MID_SET_TEXT 300140,'&Delete',50,1,12,3,0 */
/* exec SP_MID_SET_TEXT 300150,'Cle&ar',50,1,12,3,0 */
/* exec SP_MID_SET_TEXT 300160,'&Find',50,1,12,3,0 */
exec SP_MID_SET_TEXT 300101,'Cu&t',50,1,12,3,0
exec SP_MID_SET_TEXT 300102,'&Copy',50,1,12,3,0
exec SP_MID_SET_TEXT 300103,'&Paste',50,1,12,3,0
exec SP_MID_SET_TEXT 300104,'&Delete',50,1,12,3,0
exec SP_MID_SET_TEXT 300105,'Cle&ar',50,1,12,3,0
exec SP_MID_SET_TEXT 300106,'&Find',50,1,12,3,0
exec SP_MID_SET_TEXT 300107,'Recover',50,1,12,3,0
exec SP_MID_SET_TEXT 300108,'Assign',50,1,12,3,0
exec SP_MID_SET_TEXT 300109,'Unassign',50,1,12,3,0
/* End Track #4815 */
exec SP_MID_SET_TEXT 300110,'&Undo',50,1,12,3,0
exec SP_MID_SET_TEXT 300111,'R&eplace',50,1,12,3,0
/* Begin TT#335 - JSmith - Menu should say Remove and not Delete */
exec SP_MID_SET_TEXT 300112,'Remove',50,1,12,3,0
/* End TT#335 */
exec SP_MID_SET_TEXT 300200,'&View',50,1,12,3,0
exec SP_MID_SET_TEXT 300210,'Allocation Workspace',50,1,12,3,0
exec SP_MID_SET_TEXT 300220,'Merchandise Explorer',50,1,12,3,0
exec SP_MID_SET_TEXT 300230,'Store Explorer',50,1,12,3,0
exec SP_MID_SET_TEXT 300240,'Workflow/Method Explorer',50,1,12,3,0
exec SP_MID_SET_TEXT 300250,'Store Filters',50,1,12,3,0
exec SP_MID_SET_TEXT 300251,'Header Filters',50,1,12,3,0 /* TT#1313-MD -jsobek -Header Filters */
exec SP_MID_SET_TEXT 300252,'Assortment Filters',50,1,12,3,0 /* TT#1313-MD -jsobek -Header Filters */
exec SP_MID_SET_TEXT 300260,'Task List Explorer',50,1,12,3,0
exec SP_MID_SET_TEXT 300270,'Assortment Workspace',50,1,12,3,0  /* TT#2 - Ron Matelic - Assortment Planning */
exec SP_MID_SET_TEXT 300300,'&Allocation',50,1,12,3,0
exec SP_MID_SET_TEXT 300301,'&Scheduler',50,1,12,3,0
exec SP_MID_SET_TEXT 300302,'Re&view',50,1,12,3,0
exec SP_MID_SET_TEXT 300303,'&Select',50,1,12,3,0
exec SP_MID_SET_TEXT 300304,'Style',50,1,12,3,0
exec SP_MID_SET_TEXT 300305,'Summary',50,1,12,3,0
exec SP_MID_SET_TEXT 300306,'Size',50,1,12,3,0
/*Begin Assortment Planning - JScott - Assortment Planning Changes */
exec SP_MID_SET_TEXT 300307,'Asso&rtment',50,1,12,3,0
/*End Assortment Planning - JScott - Assortment Planning Changes */
exec SP_MID_SET_TEXT 300400,'&OTS Forecast',50,1,12,3,0
exec SP_MID_SET_TEXT 300410,'Re&view',50,1,12,3,0
exec SP_MID_SET_TEXT 300420,'V&iew',50,1,12,3,0 /* MID Track #4386 - Justin Bolles - Delete Views */
exec SP_MID_SET_TEXT 300500,'A&dministration',50,1,12,3,0
exec SP_MID_SET_TEXT 300510,'Securit&y',50,1,12,3,0
exec SP_MID_SET_TEXT 300520,'&Calendar',50,1,12,3,0
exec SP_MID_SET_TEXT 300530,'&Header Characteristics',50,1,12,3,0
exec SP_MID_SET_TEXT 300540,'&Store',50,1,12,3,0
exec SP_MID_SET_TEXT 300541,'&Profiles',50,1,12,3,0
exec SP_MID_SET_TEXT 300542,'&Characteristics',50,1,12,3,0
exec SP_MID_SET_TEXT 300550,'&Options',50,1,12,3,0
exec SP_MID_SET_TEXT 300560,'&Models',50,1,12,3,0
exec SP_MID_SET_TEXT 300561,'&Eligibility',50,1,12,3,0
exec SP_MID_SET_TEXT 300562,'&Stock Modifier',50,1,12,3,0
exec SP_MID_SET_TEXT 300563,'S&ales Modifier',50,1,12,3,0
exec SP_MID_SET_TEXT 300564,'&Forecasting',50,1,12,3,0
exec SP_MID_SET_TEXT 300565,'Forecast &Balance',50,1,12,3,0
exec SP_MID_SET_TEXT 300566,'S&ize Constraints',50,1,12,3,0
/* exec SP_MID_SET_TEXT 300567,'Size &Fringe',50,1,12,3,0 */      /* MID Track 3619 Remove Fringe */
exec SP_MID_SET_TEXT 300568,'Size &Alternates',50,1,12,3,0
exec SP_MID_SET_TEXT 300569,'F&WOS Override',50,1,12,3,0
exec SP_MID_SET_TEXT 300580,'&Size',50,1,12,3,0
exec SP_MID_SET_TEXT 300581,'Si&ze Groups',50,1,12,3,0
exec SP_MID_SET_TEXT 300582,'Size Cur&ves',50,1,12,3,0
exec SP_MID_SET_TEXT 300590,'&Product Characteristics',50,1,12,3,0
exec SP_MID_SET_TEXT 300600,'&Tools',50,1,12,3,0
exec SP_MID_SET_TEXT 300610,'&Sort',50,1,12,3,0
exec SP_MID_SET_TEXT 300620,'&Filter',50,1,12,3,0
exec SP_MID_SET_TEXT 300630,'Filter Wi&zard',50,1,12,3,0
exec SP_MID_SET_TEXT 300640,'Au&dit',50,1,12,3,0
exec SP_MID_SET_TEXT 300645,'Audit - Reclass',50,1,12,3,0
exec SP_MID_SET_TEXT 300650,'&Release Resources',50,1,12,3,0
exec SP_MID_SET_TEXT 300660,'&Refresh',50,1,12,3,0
/*BEGIN TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis */ 
exec [dbo].[SP_MID_SET_TEXT] 300665,'Allocation Analysis',50,1,12,3,0
exec [dbo].[SP_MID_SET_TEXT] 300666,'Forecast Analysis',50,1,12,3,0
/*END TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis */ 
exec SP_MID_SET_TEXT 300670,'&Text Editor',50,1,12,3,0
/*BEGIN TT#506 -jsobek –Add infrastructure to allow Email to be sent from the application */
exec [dbo].[SP_MID_SET_TEXT] 300675,'E&mail Message',50,1,12,3,0
/*END TT#506 -jsobek –Add infrastructure to allow Email to be sent from the application */
exec SP_MID_SET_TEXT 300680,'Schedule &Browser',50,1,12,3,0
exec SP_MID_SET_TEXT 300685,'T&heme',50,1,12,3,0				/* MID Track #5006 - Add Theme to Tools menu */
exec SP_MID_SET_TEXT 300690,'&User Options',50,1,12,3,0
exec SP_MID_SET_TEXT 300695,'&Quick Filter',50,1,12,3,0	/* Track 6138 */
exec SP_MID_SET_TEXT 300696,'Restore &Layout',50,1,12,3,0		/* Workspace Usability */
exec SP_MID_SET_TEXT 300697,'My Activity',50,1,12,3,0  /* TT#46 MD - JSmith - User Dashboard */		
exec SP_MID_SET_TEXT 300700,'&Window',50,1,12,3,0
exec SP_MID_SET_TEXT 300800,'&Help',50,1,12,3,0
exec SP_MID_SET_TEXT 300810,'&About Logility - RO',50,1,12,3,0
exec SP_MID_SET_TEXT 300820,'&New',50,1,12,3,0
exec SP_MID_SET_TEXT 301000,'Add &Characteristic',50,1,12,3,0
exec SP_MID_SET_TEXT 301001,'Add Characteristic &Value',50,1,12,3,0
exec SP_MID_SET_TEXT 301002,'&Store',50,1,12,3,0
exec SP_MID_SET_TEXT 301003,'&Header',50,1,12,3,0
exec SP_MID_SET_TEXT 301004,'&Product',50,1,12,3,0
exec SP_MID_SET_TEXT 301100,'&Reports',50,1,12,3,0
exec SP_MID_SET_TEXT 301110,'&Custom',50,1,12,3,0
exec SP_MID_SET_TEXT 301111,'&Override Low Levels',50,1,12,3,0
/*Begin Track #6232 - KJohnson - Incorporate Audit Reports in Version 3.0 Base*/
exec SP_MID_SET_TEXT 301112,'Node Properties Overrides',50,1,12,3,0
exec SP_MID_SET_TEXT 301113,'Forecast Audit Merchandise',50,1,12,3,0
exec SP_MID_SET_TEXT 301114,'Forecast Audit Method',50,1,12,3,0
/* Begin TT#1350 - JSmith - Missing text */
exec SP_MID_SET_TEXT 301115,'Allocation Audit',50,1,12,3,0
/* End TT#1350 */
/*End Track #6232 - KJohnson*/
exec SP_MID_SET_TEXT 301116,'Assortment Explorer',50,1,12,3,0		/* TT#2 - stodd- assortment */

exec SP_MID_SET_TEXT 301117,'F&WOS Max',50,1,12,3,0 /*TT#108 - MD - DOConnell - FWOS Max Model */
exec SP_MID_SET_TEXT 301119,'Group Allocation',50,1,12,3,0	/* TT#488-MD - STodd - Group Allocation */
/* BEGIN TT#739-MD -jsobek -Delete Stores -Allocation by Store Report */
exec [dbo].[SP_MID_SET_TEXT] 301120,'Allocation By Store Report',50,1,12,3,0
/* End TT#739-MD -jsobek -Delete Stores -Allocation by Store Report */
exec [dbo].[SP_MID_SET_TEXT] 301121,'Process Control',50,1,12,3,0	/* TT#1581-MD - stodd - API Header Reconcile */
/* Enums - code must match value in Enums.cs Type must match entry in eMIDTextType in Enums.cs */
exec [dbo].[SP_MID_SET_TEXT] 301122,'Scheduler Job Manager',50,1,12,3,0

/* eHierarchyType */
exec SP_MID_SET_TEXT 800000,'Organizational',30,2,null,null,0
exec SP_MID_SET_TEXT 800001,'Alternate',30,2,null,null,0

/* eLevelLengthType */
exec SP_MID_SET_TEXT 800100,'Unrestricted',30,3,null,null,0
exec SP_MID_SET_TEXT 800101,'Required',30,3,null,null,0
exec SP_MID_SET_TEXT 800102,'Range',30,3,null,null,0

/* eHierarchyLevelMasterType */
/* eHierarchyLevelType */
exec SP_MID_SET_TEXT 800200,'Undefined',30,4,null,null,0
exec SP_MID_SET_TEXT 800201,'Plan Level',30,4,null,null,0
exec SP_MID_SET_TEXT 800202,'Style',30,4,null,null,0
exec SP_MID_SET_TEXT 800203,'Color',30,4,null,null,0
exec SP_MID_SET_TEXT 800204,'Size',30,4,null,null,0
exec SP_MID_SET_TEXT 800205,'Parent Of Style',30,4,null,null,0

/* Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 1 */
/* eHierarchyRollupOption */
exec SP_MID_SET_TEXT 800250,'Undefined',30,4,null,null,0
exec SP_MID_SET_TEXT 800251,'Real Time',30,4,null,null,0
exec SP_MID_SET_TEXT 800252,'API',30,4,null,null,0

/* End - JScott - Add Rollup Type to Hierarchy Properties - Part 1 */
/* eProductType */
exec SP_MID_SET_TEXT 800300,'Undefined',30,5,null,null,0
exec SP_MID_SET_TEXT 800301,'Hardlines',30,5,null,null,0
exec SP_MID_SET_TEXT 800302,'Softlines',30,5,null,null,0

/* eProcesses */
exec SP_MID_SET_TEXT 800400,'Unknown',30,6,null,null,0
exec SP_MID_SET_TEXT 800401,'Hierarchy Load',30,6,null,null,0
exec SP_MID_SET_TEXT 800402,'Store Load',30,6,null,null,0
exec SP_MID_SET_TEXT 800403,'History/Forecast Load',30,6,null,null,0
exec SP_MID_SET_TEXT 800404,'Client Application',30,6,null,null,0
exec SP_MID_SET_TEXT 800405,'Store Attribute Builder',30,6,null,null,0
exec SP_MID_SET_TEXT 800406,'Hierarchy Web Service',30,6,null,null,0
exec SP_MID_SET_TEXT 800407,'Color Codes Load',30,6,null,null,0
exec SP_MID_SET_TEXT 800408,'Size Codes Load',30,6,null,null,0
exec SP_MID_SET_TEXT 800409,'Header Load',30,6,null,null,0
exec SP_MID_SET_TEXT 800410,'Forecasting',30,6,null,null,0
exec SP_MID_SET_TEXT 800411,'Rollup',30,6,null,null,0
exec SP_MID_SET_TEXT 800412,'Control Service',30,6,null,null,0
exec SP_MID_SET_TEXT 800413,'Application Service',30,6,null,null,0
exec SP_MID_SET_TEXT 800414,'Hierarchy Service',30,6,null,null,0
exec SP_MID_SET_TEXT 800415,'Store Service',30,6,null,null,0
exec SP_MID_SET_TEXT 800416,'Relieve Intransit',30,6,null,null,0
exec SP_MID_SET_TEXT 800417,'Purge',30,6,null,null,0
exec SP_MID_SET_TEXT 800418,'Allocate',30,6,null,null,0
exec SP_MID_SET_TEXT 800419,'Scheduler Service',30,6,null,null,0
exec SP_MID_SET_TEXT 800420,'External Program',30,6,null,null,0
exec SP_MID_SET_TEXT 800421,'Header Service',30,6,null,null,0
exec SP_MID_SET_TEXT 800422,'Size Curve Load',30,6,null,null,0
exec SP_MID_SET_TEXT 800423,'SQL Script',30,6,null,null,0
exec SP_MID_SET_TEXT 800424,'Matrix Balance',30,6,null,null,0
exec SP_MID_SET_TEXT 800425,'Database Conversion Utility',30,6,null,null,0
exec SP_MID_SET_TEXT 800426,'Rebuild Intransit',30,6,null,null,0   /* TT#1137 (MID Track 4351) Rebuild Intransit Utility */
exec SP_MID_SET_TEXT 800427,'Size Constraints Load',30,6,null,null,0
exec SP_MID_SET_TEXT 800428,'Chain Forecasting',30,6,null,null,0
/* Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached. */
exec SP_MID_SET_TEXT 800430,'Forecast Export Thread',30,6,null,null,0
/* End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached. */
exec SP_MID_SET_TEXT 800429,'Special Request',30,6,null,null,0 /* Issue 5117 */
/* Begin TT#155 - JSmith - Size Curve Method */
exec SP_MID_SET_TEXT 800432,'Size Curve Generate',30,6,null,null,0
exec SP_MID_SET_TEXT 800433,'Size Curve Method',30,6,null,null,0
exec SP_MID_SET_TEXT 800434,'Size Curve',30,6,null,null,0
/* End TT#155 - JSmith - Size Curve Method */
/* Begin TT#391 - STodd - Size Day To Week Summary */
exec SP_MID_SET_TEXT 800435,'Size Day To Week Summary',30,6,null,null,0
exec SP_MID_SET_TEXT 800437,'Store Bin Viewer',30,6,null,null,0
/* End TT#391 - STodd - Size Day To Week Summary */
/* Begin MOD - JScott - Build Pack Criteria Load */
exec SP_MID_SET_TEXT 800436,'Build Pack Criteria Load',30,6,null,null,0
/* End MOD - JScott - Build Pack Criteria Load */
/* Begin TT#707 - JScott - Size Curve process needs to multi-thread */
exec SP_MID_SET_TEXT 800438,'Size Curve Generate Thread',30,6,null,null,0
/* End TT#707 - JScott - Size Curve process needs to multi-thread */
/* Begin TT#710 - JSmith - Generate relieve intransit */
exec SP_MID_SET_TEXT 800439,'Generate Relieve Intransit',30,6,null,null,0
/* End TT#710 */
/* Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model */
exec SP_MID_SET_TEXT 800440,'Determine Hierarchy Activity',30,6,null,null,0
/* End TT#988 */
/* Begin TT#2362 - AGallagher - Bad or Missing application text code task in Task List */
exec SP_MID_SET_TEXT 800441,'Chain Set Percent Criteria Load',30,6,null,null,0
/* End TT#2362 */
-- Begin TT#1463 - JScott - Alternate Hierarchy Reclass
exec SP_MID_SET_TEXT 800442,'Hierarchy Reclass',30,6,null,null,0
-- End TT#1463 - JScott - Alternate Hierarchy Reclass
/* BEGIN TT#1401 - stodd - add resevation stores (IMO)	*/
exec SP_MID_SET_TEXT 800444,'Push to Back-stock',30,6,null,null,0
/* BEGIN TT#1401 - stodd - add resevation stores (IMO)	*/
-- Begin TT#2249 - JSmith - Wrong name for process
exec SP_MID_SET_TEXT 800445,'Header Allocation Load',30,6,null,null,0
-- End TT#2249
/* begin TT#43 - stodd -projected sales */
exec SP_MID_SET_TEXT 800446,'DailyPercentages',30,6,null,null,0	
/* END TT#43 - stodd -projected sales */
exec SP_MID_SET_TEXT 800447,'Schedule Interface',30,6,null,null,0   --TT#240 MD - JSmith - Change Scheduler Interface to have its own audit entry and not report as Scheduler Service
exec SP_MID_SET_TEXT 800448,'Convert Filters',30,6,null,null,0  --TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
exec SP_MID_SET_TEXT 800449,'Store Delete',30,6,null,null,0  --TT#739-MD - stodd - delete stores

exec SP_MID_SET_TEXT 800450,'Store Eligibility Load',30,6,null,null,0  --TT#816 -MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
exec SP_MID_SET_TEXT 800451,'VSW Load',30,6,null,null,0  --TT#817 -MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API
exec SP_MID_SET_TEXT 800452,'Header Reconcile',30,6,null,null,0		--  TT#1581-MD - stodd - API Header Reconcile
/* Begin TT#1670-MD - RMatelic Data Layer Request - Remove Application Text for 'Allocation Task List */
/* exec SP_MID_SET_TEXT 800453,'Allocation Tasklist',30,6,null,null,0	--  TT#1581-MD - stodd - API Header Reconcile */
/* End TT#1670-MD */
exec SP_MID_SET_TEXT 800454,'Batch Comp',30,6,null,null,0	--TT#1595-MD - stodd - Batch Comp
exec SP_MID_SET_TEXT 800455,'Scheduler Job Manager',30,6,null,null,0
exec SP_MID_SET_TEXT 800456,'Planning Extract Thread',30,6,null,null,0   /* TT#2131-MD - JSmith - Halo Integration */

/* eProcessStatus */
/*
exec SP_MID_SET_TEXT 800500,'New',30,7,null,null,0
exec SP_MID_SET_TEXT 800501,'Completed',30,7,null,null,0
exec SP_MID_SET_TEXT 800502,'Active',30,7,null,null,0
exec SP_MID_SET_TEXT 800503,'Failed',30,7,null,null,0
exec SP_MID_SET_TEXT 800504,'Scheduled',30,7,null,null,0
*/

/* eProcessCompletionStatus */
exec SP_MID_SET_TEXT 800500,'None',30,7,null,null,0
exec SP_MID_SET_TEXT 800501,'Successful',30,7,null,null,0
exec SP_MID_SET_TEXT 800503,'Failed',30,7,null,null,0
exec SP_MID_SET_TEXT 800504,'Condition Failed',30,7,null,null,0
exec SP_MID_SET_TEXT 800505,'Cancelled',30,7,null,null,0
exec SP_MID_SET_TEXT 800506,'Unexpected Termination',30,7,null,null,0

/* eOTSPlanLevelType */
exec SP_MID_SET_TEXT 800600,'Not Defined',30,8,null,null,0
exec SP_MID_SET_TEXT 800601,'Regular',30,8,null,null,0
exec SP_MID_SET_TEXT 800602,'Total',30,8,null,null,0

/* eStoreDisplayOptions */
exec SP_MID_SET_TEXT 800650,'Name',30,13,null,null,0
exec SP_MID_SET_TEXT 800651,'ID',30,13,null,null,0
exec SP_MID_SET_TEXT 800652,'Description',30,13,null,null,0
exec SP_MID_SET_TEXT 800653,'ID and Name',30,13,null,null,0
exec SP_MID_SET_TEXT 800654,'ID and Description',30,13,null,null,0
exec SP_MID_SET_TEXT 800655,'Name and Description',30,13,null,null,0
exec SP_MID_SET_TEXT 800656,'ID and Name and Desc',30,13,null,null,0

/* eForecastFomulaType MID Track #5773 - Planned FWOS Enhancement */
/* TT#248 - change TEXT_TYPE from 13 to 89 */
exec SP_MID_SET_TEXT 800661,'Stock',30,89,null,null,0
exec SP_MID_SET_TEXT 800662,'Sales',30,89,null,null,0
exec SP_MID_SET_TEXT 800663,'Percent Contribution',30,89,null,null,0

/* eHierarchyDisplayOptions */
exec SP_MID_SET_TEXT 800700,'Name',30,21,null,null,0
exec SP_MID_SET_TEXT 800701,'ID',30,21,null,null,0
exec SP_MID_SET_TEXT 800702,'Description',30,21,null,null,0
exec SP_MID_SET_TEXT 800703,'ID and Name',30,21,null,null,0
exec SP_MID_SET_TEXT 800704,'ID and Description',30,21,null,null,0
exec SP_MID_SET_TEXT 800705,'Name and Description',30,21,null,null,0
exec SP_MID_SET_TEXT 800706,'ID and Name and Desc',30,21,null,null,0
exec SP_MID_SET_TEXT 800707,'Do not display on explorer',30,21,null,null,0

/* eStorePlanSelectedGroupBy */
exec SP_MID_SET_TEXT 800720,'Time Period',30,22,null,null,0
exec SP_MID_SET_TEXT 800721,'Variable',30,22,null,null,0

/* eHeaderType */
exec SP_MID_SET_TEXT 800730,'Receipt',20,31,null,null,0
exec SP_MID_SET_TEXT 800731,'ASN',20,31,null,null,0
exec SP_MID_SET_TEXT 800732,'Dummy',20,31,null,null,0
exec SP_MID_SET_TEXT 800733,'Drop Ship',20,31,null,null,0
exec SP_MID_SET_TEXT 800734,'Multi-Header',20,31,null,null,0
exec SP_MID_SET_TEXT 800735,'Reserve',20,31,null,null,0
/*exec SP_MID_SET_TEXT 800736,'Workup Size Buy',20,31,null,null,0 */  /* MID Track 3891 Remove Work Up Size Buy (now part of Work Up Total Buy)  */
exec SP_MID_SET_TEXT 800737,'Workup Total Buy',20,31,null,null,0
exec SP_MID_SET_TEXT 800738,'Purchase Order',20,31,null,null,0
exec SP_MID_SET_TEXT 800739,'Assortment',20,31,null,null,0
exec SP_MID_SET_TEXT 800740,'Placeholder',20,31,null,null,0
exec SP_MID_SET_TEXT 800741,'VSW',20,31,null,null,0                /* TT#1401 – JEllis – Urban Reservation Stores pt 2 */
exec SP_MID_SET_TEXT 800742,'Master',20,31,null,null,0    /* TT#1966-MD - JSmith - DC Fulfillment */

/* ePackType */
exec SP_MID_SET_TEXT 800745,'Generic',20,32,null,null,0
exec SP_MID_SET_TEXT 800746,'Detail',20,32,null,null,0

/* eComponentType */
exec SP_MID_SET_TEXT 800750,'Total',30,24,null,null,0
exec SP_MID_SET_TEXT 800751,'Generic Type',30,24,null,null,0
exec SP_MID_SET_TEXT 800752,'Detail Type',30,24,null,null,0
exec SP_MID_SET_TEXT 800753,'All Packs',30,24,null,null,0
exec SP_MID_SET_TEXT 800754,'All Generic Packs',30,24,null,null,0
exec SP_MID_SET_TEXT 800755,'All Detail Packs',30,24,null,null,0
exec SP_MID_SET_TEXT 800756,'Specific Pack',30,24,null,null,0
exec SP_MID_SET_TEXT 800757,'Bulk',30,24,null,null,0
exec SP_MID_SET_TEXT 800758,'All Colors',30,24,null,null,0
exec SP_MID_SET_TEXT 800759,'Specific Color',30,24,null,null,0
exec SP_MID_SET_TEXT 800760,'All Sizes',30,24,null,null,0
exec SP_MID_SET_TEXT 800761,'Specific Size',30,24,null,null,0
exec SP_MID_SET_TEXT 800762,'Color and Size',30,24,null,null,0
exec SP_MID_SET_TEXT 800763,'Matching Pack',30,24,null,null,0
exec SP_MID_SET_TEXT 800764,'Matching Color',30,24,null,null,0
exec SP_MID_SET_TEXT 800765,'Specific Size Primary Dim',30,24,null,null,0
exec SP_MID_SET_TEXT 800766,'Specific Size Secondary Dim',30,24,null,null,0
exec SP_MID_SET_TEXT 800767,'Detail Sub Type',30,24,null,null,0

exec SP_MID_SET_TEXT 800770,'Detail',30,24,null,null,0
exec SP_MID_SET_TEXT 800771,'Received',30,24,null,null,0
exec SP_MID_SET_TEXT 800772,'Balance',30,24,null,null,0
exec SP_MID_SET_TEXT 800773,'# Stores',30,24,null,null,0
exec SP_MID_SET_TEXT 800774,'Avg',30,24,null,null,0
exec SP_MID_SET_TEXT 800775,'Total',30,24,null,null,0

/* eStoreAllocationNode */
exec SP_MID_SET_TEXT 800781,'Store Detail Total',30,37,null,null,0
exec SP_MID_SET_TEXT 800782,'Set Filter Total',30,37,null,null,0
exec SP_MID_SET_TEXT 800783,'Set Total',30,37,null,null,0
exec SP_MID_SET_TEXT 800784,'Filter Total',30,37,null,null,0
exec SP_MID_SET_TEXT 800785,'All Store Total',30,37,null,null,0

/* eStoreAllocationNode */
exec SP_MID_SET_TEXT 800791,'Header Detail',30,36,null,null,0
exec SP_MID_SET_TEXT 800792,'Header Subtotal',30,36,null,null,0
exec SP_MID_SET_TEXT 800793,'Header Grand Total',30,36,null,null,0

/* eMethodType */
/*
exec SP_MID_SET_TEXT 800800,'OTS Forecast',30,10,null,null,0
exec SP_MID_SET_TEXT 800801,'Forecast Balance',30,10,null,null,0
exec SP_MID_SET_TEXT 800802,'General Allocation',30,10,null,null,0
exec SP_MID_SET_TEXT 800803,'Allocation Override',30,10,null,null,0
exec SP_MID_SET_TEXT 800804,'Velocity',30,10,null,null,0
exec SP_MID_SET_TEXT 800805,'Rule',30,10,null,null,0
exec SP_MID_SET_TEXT 800806,'Fill Size Holes Allocation',30,10,null,null,0
exec SP_MID_SET_TEXT 800807,'Basis Size Allocation',30,10,null,null,0
exec SP_MID_SET_TEXT 800808,'Warehouse Size Allocation',30,10,null,null,0
exec SP_MID_SET_TEXT 800809,'Size Need Allocation',30,10,null,null,0
exec SP_MID_SET_TEXT 800810,'Size Override Allocation',30,10,null,null,0
*/

/* eGroupLevelFunctionType (OTS PLan Method types,0*/
exec SP_MID_SET_TEXT 800811,'% Contribution',30,11,null,null,0
exec SP_MID_SET_TEXT 800812,'Trend',30,11,null,null,0
exec SP_MID_SET_TEXT 800813,'Average Sales',30,11,null,null,0
exec SP_MID_SET_TEXT 800814,'Current Trend',30,11,null,null,0

/* eGroupLevelSmoothBy */
exec SP_MID_SET_TEXT 800820,'Store Set',30,12,null,null,0
exec SP_MID_SET_TEXT 800821,'Store Grade',30,12,null,null,0
exec SP_MID_SET_TEXT 800822,'Both',30,12,null,null,0
exec SP_MID_SET_TEXT 800823,'<None>',30,12,null,null,0

/* eWorkflowType */
exec SP_MID_SET_TEXT 800825,'OTS Forecast',30,27,null,null,0
exec SP_MID_SET_TEXT 800826,'Allocation',30,27,null,null,0
exec SP_MID_SET_TEXT 800827,'Assortment',30,27,null,null,0  -- Assortment

/* eGlobalUserType */
exec SP_MID_SET_TEXT 800830,'User',30,28,null,null,0
exec SP_MID_SET_TEXT 800831,'Global',30,28,null,null,0

/* eWorkflowMethodIND */
exec SP_MID_SET_TEXT 800835,'Methods',30,29,null,null,0
exec SP_MID_SET_TEXT 800836,'Workflows',30,29,null,null,0
exec SP_MID_SET_TEXT 800837,'Size Methods',30,29,null,null,0

/* eTyLyType */
exec SP_MID_SET_TEXT 800840,'Non TY/LY',30,14,null,null,0
exec SP_MID_SET_TEXT 800841,'TY/LY',30,14,null,null,0
exec SP_MID_SET_TEXT 800842,'Alternate LY',30,14,null,null,0
exec SP_MID_SET_TEXT 800843,'Alternate Apply To',30,14,null,null,0

/* eCalendarDateType */
exec SP_MID_SET_TEXT 800850,'Day',30,15,null,null,0
exec SP_MID_SET_TEXT 800851,'Week',30,15,null,null,0
exec SP_MID_SET_TEXT 800852,'Month',30,15,null,null,0
exec SP_MID_SET_TEXT 800853,'Period',30,15,null,null,0
exec SP_MID_SET_TEXT 800860,'Year',30,15,null,null,0

/* eWeek53Offset */
exec SP_MID_SET_TEXT 800854,'Drop Week 53',30,15,null,null,0
exec SP_MID_SET_TEXT 800855,'One Week Offset',30,15,null,null,0

/* eCalendarRangeType */
exec SP_MID_SET_TEXT 800857,'Static',30,15,null,null,0
exec SP_MID_SET_TEXT 800858,'Dynamic',30,15,null,null,0
exec SP_MID_SET_TEXT 800859,'Reccurring',30,15,null,null,0
exec SP_MID_SET_TEXT 800861,'Dynamic Switch',30,15,null,null,0

/* BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)	*/
/* eAdjustVSW */
exec SP_MID_SET_TEXT 800865,'Replace',30,92,null,null,0
exec SP_MID_SET_TEXT 800866,'Adjust',30,92,null,null,0
/* END TT#2225 - stodd - VSW ANF Enhancement (IMO)	*/

/* eTrendCapID */
exec SP_MID_SET_TEXT 800870,'None',30,73,null,null,0
exec SP_MID_SET_TEXT 800871,'Tolerance',30,73,null,null,0
exec SP_MID_SET_TEXT 800872,'Limits',30,73,null,null,0

/* ePeriondName */
exec SP_MID_SET_TEXT 800873,'February',30,71,null,null,0
exec SP_MID_SET_TEXT 800874,'March',30,71,null,null,0
exec SP_MID_SET_TEXT 800875,'April',30,71,null,null,0
exec SP_MID_SET_TEXT 800876,'May',30,71,null,null,0
exec SP_MID_SET_TEXT 800877,'June',30,71,null,null,0
exec SP_MID_SET_TEXT 800878,'July',30,71,null,null,0
exec SP_MID_SET_TEXT 800879,'August',30,71,null,null,0
exec SP_MID_SET_TEXT 800880,'September',30,71,null,null,0
exec SP_MID_SET_TEXT 800881,'October',30,71,null,null,0
exec SP_MID_SET_TEXT 800882,'November',30,71,null,null,0
exec SP_MID_SET_TEXT 800883,'December',30,71,null,null,0
exec SP_MID_SET_TEXT 800884,'January',30,71,null,null,0

/* ePeriondAbbreviation */
exec SP_MID_SET_TEXT 800886,'Feb',30,72,null,null,0
exec SP_MID_SET_TEXT 800887,'Mar',30,72,null,null,0
exec SP_MID_SET_TEXT 800888,'Apr',30,72,null,null,0
exec SP_MID_SET_TEXT 800889,'May',30,72,null,null,0
exec SP_MID_SET_TEXT 800890,'Jun',30,72,null,null,0
exec SP_MID_SET_TEXT 800891,'Jul',30,72,null,null,0
exec SP_MID_SET_TEXT 800892,'Aug',30,72,null,null,0
exec SP_MID_SET_TEXT 800893,'Sep',30,72,null,null,0
exec SP_MID_SET_TEXT 800894,'Oct',30,72,null,null,0
exec SP_MID_SET_TEXT 800895,'Nov',30,72,null,null,0
exec SP_MID_SET_TEXT 800896,'Dec',30,72,null,null,0
exec SP_MID_SET_TEXT 800897,'Jan',30,72,null,null,0

/* eHierarchyIDFormat */
exec SP_MID_SET_TEXT 800900,'Unique',30,25,null,null,0
exec SP_MID_SET_TEXT 800901,'Combine',30,25,null,null,0

/* eCalendarModelPeriodType */
exec SP_MID_SET_TEXT 800905,'None',30,0,null,null,0
exec SP_MID_SET_TEXT 800906,'Month',30,0,null,null,0
exec SP_MID_SET_TEXT 800907,'Quarter',30,0,null,null,0
exec SP_MID_SET_TEXT 800908,'Season',30,0,null,null,0
exec SP_MID_SET_TEXT 800909,'Year',30,0,null,null,0

/* eQuarterName */
exec SP_MID_SET_TEXT 800911,'1st Quarter',30,82,null,null,0
exec SP_MID_SET_TEXT 800912,'2nd Quarter',30,82,null,null,0
exec SP_MID_SET_TEXT 800913,'3rd Quarter',30,82,null,null,0
exec SP_MID_SET_TEXT 800914,'4th Quarter',30,82,null,null,0

/* eQuarterAbbrev */
exec SP_MID_SET_TEXT 800915,'1st Qtr',30,83,null,null,0
exec SP_MID_SET_TEXT 800916,'2nd Qtr',30,83,null,null,0
exec SP_MID_SET_TEXT 800917,'3rd Qtr',30,83,null,null,0
exec SP_MID_SET_TEXT 800918,'4th Qtr',30,83,null,null,0

/* eSeasonName */
exec SP_MID_SET_TEXT 800919,'Spring',30,84,null,null,0
exec SP_MID_SET_TEXT 800920,'Fall',30,84,null,null,0

/* eSeasonAbbrev */
exec SP_MID_SET_TEXT 800921,'Spr',30,85,null,null,0
exec SP_MID_SET_TEXT 800922,'Fall',30,85,null,null,0

/* ePurgeTimeframe */
exec SP_MID_SET_TEXT 800950,'<None>',30,30,null,null,0
exec SP_MID_SET_TEXT 800951,'Days',30,30,null,null,0
exec SP_MID_SET_TEXT 800952,'Weeks',30,30,null,null,0


/* Security enums 801000 - 801999 */
/* eSecurityLevel */
exec SP_MID_SET_TEXT 801000,'Not Specified',30,0,null,null,0
exec SP_MID_SET_TEXT 801004,'Allow',30,0,null,null,0
exec SP_MID_SET_TEXT 801005,'Deny',30,0,null,null,0

/* eValidUsername */
exec SP_MID_SET_TEXT 801010,'Valid',30,0,null,null,0
exec SP_MID_SET_TEXT 801011,'Invalid Too Short',30,0,null,null,0
exec SP_MID_SET_TEXT 801012,'Invalid Illegal Char',30,0,null,null,0
exec SP_MID_SET_TEXT 801013,'User Already Exists',30,0,null,null,0

/* eValidPassword */
exec SP_MID_SET_TEXT 801020,'Valid',30,0,null,null,0
exec SP_MID_SET_TEXT 801021,'Invalid Too Short',30,0,null,null,0
exec SP_MID_SET_TEXT 801022,'Invalid Illegal Char',30,0,null,null,0

/* eValidGroupname */
exec SP_MID_SET_TEXT 801030,'Valid',30,0,null,null,0
exec SP_MID_SET_TEXT 801031,'Invalid Too Short',30,0,null,null,0
exec SP_MID_SET_TEXT 801032,'Invalid Illegal Char',30,0,null,null,0
exec SP_MID_SET_TEXT 801033,'Group Already Exists',30,0,null,null,0

/* eSecurityAuthenticate */
exec SP_MID_SET_TEXT 801150,'Incorrect Password',30,17,null,null,0
exec SP_MID_SET_TEXT 801151,'Unknown User',30,17,null,null,0
exec SP_MID_SET_TEXT 801152,'Inactive User',30,17,null,null,0
exec SP_MID_SET_TEXT 801153,'Active User',30,17,null,null,0
exec SP_MID_SET_TEXT 801154,'User Authenticated',30,17,null,null,0
exec SP_MID_SET_TEXT 801155,'Password Changed',30,17,null,null,0
exec SP_MID_SET_TEXT 801156,'Process is Unavailable',30,17,null,null,0	/* TT#1581-MD -stodd - Header Reconcile. */

/* eSecurityActions */
exec SP_MID_SET_TEXT 801160,'Full Control',50,66,null,null,0
exec SP_MID_SET_TEXT 801161,'Maintain',50,66,null,null,0
exec SP_MID_SET_TEXT 801162,'Delete',50,66,null,null,0
exec SP_MID_SET_TEXT 801163,'View',50,66,null,null,0
exec SP_MID_SET_TEXT 801164,'Execute',50,66,null,null,0
exec SP_MID_SET_TEXT 801165,'Reclassify',50,66,null,null,0
exec SP_MID_SET_TEXT 801166,'Add Sets',50,66,null,null,0
exec SP_MID_SET_TEXT 801167,'Inactivate',50,66,null,null,0
exec SP_MID_SET_TEXT 801168,'Interactive',50,66,null,null,0
/*Begin Track #4961 - JSmith - Add security for apply to lower levels. */
/* exec SP_MID_SET_TEXT 801169,'Apply To Lower Levels',30,66,null,null,0 TT#1337 - RBeck - Schemas Different Across Brands */  
exec SP_MID_SET_TEXT 801169,'Apply To Lower Levels',30,0,null,null,0
/*exec SP_MID_SET_TEXT 801170,'Inherit From Higher Level',30,66,null,null,0 TT#1337 - RBeck - Schemas Different Across Brands*/
exec SP_MID_SET_TEXT 801170,'Inherit From Higher Level',30,0,null,null,0
/*End Track #4961  */
/* Begin Track #4815 - JSmith - #283-User (Security) Maintenance */
exec SP_MID_SET_TEXT 801171,'Assign',50,66,null,null,0 
/* End Track #4815 */
exec SP_MID_SET_TEXT 801173,'Apply Changes To Lower Levels',30,66,null,null,0  /* TT#2015 - JSmith - Apply Changes to Lower Level */


--Begin TT#1076 - JScott - Size Curves by Set
exec SP_MID_SET_TEXT 801200,'None',30,0,null,null,0
exec SP_MID_SET_TEXT 801201,'Store',30,0,null,null,0
exec SP_MID_SET_TEXT 801202,'Attribute Set',30,0,null,null,0

--End TT#1076 - JScott - Size Curves by Set
/*eFringeOverrideOperator*/
/*exec SP_MID_SET_TEXT 801400,'AND',30,61,null,null,0  */     /* MID Track 3619 Remove Fringe */
/*exec SP_MID_SET_TEXT 801401,'OR',30,61,null,null,0   */     /* MID Track 3619 Remove Fringe */

/*eFringeOverrideUnitCriteria*/
/*exec SP_MID_SET_TEXT 801410,'Received',30,62,null,null,0              */  /* MID Track 3619 Remove Fringe */
/*exec SP_MID_SET_TEXT 801411,'Remaining to Allocate',30,62,null,null,0 */  /* MID Track 3619 Remove Fringe */

/*eFringeOverrideCondition*/
/*exec SP_MID_SET_TEXT 801420,'<',30,63,null,null,0    */     /* MID Track 3619 Remove Fringe */
/*exec SP_MID_SET_TEXT 801421,'<=',30,63,null,null,0   */     /* MID Track 3619 Remove Fringe */
/*exec SP_MID_SET_TEXT 801422,'>',30,63,null,null,0    */     /* MID Track 3619 Remove Fringe */
/*exec SP_MID_SET_TEXT 801423,'>=',30,63,null,null,0   */     /* MID Track 3619 Remove Fringe */

/*eFringeOverrideSort*/
/*exec SP_MID_SET_TEXT 801440,'Ascending',30,64,null,null,0  */ /* MID Track 3619 Remove Fringe */
/*exec SP_MID_SET_TEXT 801441,'Descending',30,64,null,null,0 */ /* MID Track 3619 Remove Fringe */


/* eSecurityViews */
/*
exec SP_MID_SET_TEXT 801190,'All',30,20,null,null,0
exec SP_MID_SET_TEXT 801191,'Sales',30,20,null,null,0
exec SP_MID_SET_TEXT 801192,'Stock',30,20,null,null,0
exec SP_MID_SET_TEXT 801193,'Markdowns',30,20,null,null,0
exec SP_MID_SET_TEXT 801194,'Intransit',30,20,null,null,0
exec SP_MID_SET_TEXT 801195,'WeeksOfSupply',30,20,null,null,0
*/

/* eTaskType */
/*
exec SP_MID_SET_TEXT 801500,'Allocation',30,39,null,null,0
exec SP_MID_SET_TEXT 801501,'OTS Forecast',30,39,null,null,0
exec SP_MID_SET_TEXT 801502,'Rollup',30,39,null,null,0
*/

/* eTaskStatus */
/*
exec SP_MID_SET_TEXT 801510,'Submitted',30,40,null,null,0
exec SP_MID_SET_TEXT 801511,'Started',30,40,null,null,0
exec SP_MID_SET_TEXT 801512,'Review',30,40,null,null,0
exec SP_MID_SET_TEXT 801513,'Completed',30,40,null,null,0
exec SP_MID_SET_TEXT 801514,'Failed',30,40,null,null,0
*/

/* eScheduleTimeUnit */
/*
exec SP_MID_SET_TEXT 801520,'Hour',30,41,null,null,0
exec SP_MID_SET_TEXT 801521,'Day',30,41,null,null,0
exec SP_MID_SET_TEXT 801522,'Week',30,41,null,null,0
exec SP_MID_SET_TEXT 801523,'Month',30,41,null,null,0
*/

/* eSceduleType */
/*
exec SP_MID_SET_TEXT 801530,'Immediate Local',30,42,null,null,0
exec SP_MID_SET_TEXT 801531,'Immediate Remote',30,42,null,null,0
exec SP_MID_SET_TEXT 801532,'Schedule One Time',30,42,null,null,0
exec SP_MID_SET_TEXT 801533,'Schedule Recurring',30,42,null,null,0
*/

/* eTaskPriority */
/*
exec SP_MID_SET_TEXT 801540,'Low',30,43,null,null,0
exec SP_MID_SET_TEXT 801541,'Medium',30,43,null,null,0
exec SP_MID_SET_TEXT 801542,'High',30,43,null,null,0
*/

/* eAllocationTaskType */
/*
exec SP_MID_SET_TEXT 801550,'Reserve',30,44,null,null,0
exec SP_MID_SET_TEXT 801551,'Header',30,44,null,null,0
exec SP_MID_SET_TEXT 801552,'PO',30,44,null,null,0
*/

/* eUnitScaling */
/*Begin Modification - JScott - Add Scaling Decimals */
/*exec SP_MID_SET_TEXT 801501,'10',30,74,null,null,0*/
/*exec SP_MID_SET_TEXT 801502,'100',30,74,null,null,0*/
/*exec SP_MID_SET_TEXT 801503,'1000',30,74,null,null,0*/
/*exec SP_MID_SET_TEXT 801504,'N/A',30,74,null,null,0*/
/*exec SP_MID_SET_TEXT 801505,'N/A',30,74,null,null,0*/
/*exec SP_MID_SET_TEXT 801506,'N/A',30,74,null,null,0*/
/*exec SP_MID_SET_TEXT 801507,'N/A',30,74,null,null,0*/
/*exec SP_MID_SET_TEXT 801508,'N/A',30,74,null,null,0*/
/*exec SP_MID_SET_TEXT 801509,'N/A',30,74,null,null,0*/
exec SP_MID_SET_TEXT 801501,'10',30,74,null,null,0
exec SP_MID_SET_TEXT 801502,'100',30,74,null,null,0
exec SP_MID_SET_TEXT 801503,'100.0',30,74,null,null,0
exec SP_MID_SET_TEXT 801504,'1000',30,74,null,null,0
exec SP_MID_SET_TEXT 801505,'1000.0',30,74,null,null,0
exec SP_MID_SET_TEXT 801506,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801507,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801508,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801509,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801510,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801511,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801512,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801513,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801514,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801515,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801516,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801517,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801518,'',30,74,null,null,0
exec SP_MID_SET_TEXT 801519,'',30,74,null,null,0
/*End Modification - JScott - Add Scaling Decimals */

/* eDollarScaling */
/*Begin Modification - JScott - Add Scaling Decimals */
/*exec SP_MID_SET_TEXT 801511,'10',30,75,null,null,0*/
/*exec SP_MID_SET_TEXT 801512,'100',30,75,null,null,0*/
/*exec SP_MID_SET_TEXT 801513,'1000',30,75,null,null,0*/
/*exec SP_MID_SET_TEXT 801514,'N/A',30,75,null,null,0*/
/*exec SP_MID_SET_TEXT 801515,'N/A',30,75,null,null,0*/
/*exec SP_MID_SET_TEXT 801516,'N/A',30,75,null,null,0*/
/*exec SP_MID_SET_TEXT 801517,'N/A',30,75,null,null,0*/
/*exec SP_MID_SET_TEXT 801518,'N/A',30,75,null,null,0*/
/*exec SP_MID_SET_TEXT 801519,'N/A',30,75,null,null,0*/
exec SP_MID_SET_TEXT 801521,'10',30,75,null,null,0
exec SP_MID_SET_TEXT 801522,'100',30,75,null,null,0
exec SP_MID_SET_TEXT 801523,'100.0',30,75,null,null,0
exec SP_MID_SET_TEXT 801524,'1000',30,75,null,null,0
exec SP_MID_SET_TEXT 801525,'1000.0',30,75,null,null,0
exec SP_MID_SET_TEXT 801526,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801527,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801528,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801529,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801530,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801531,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801532,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801533,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801534,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801535,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801536,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801537,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801538,'',30,75,null,null,0
exec SP_MID_SET_TEXT 801539,'',30,75,null,null,0
/*End Modification - JScott - Add Scaling Decimals */

/* eProcessExecutionStatus */
exec SP_MID_SET_TEXT 801600,'None',30,40,null,null,0
exec SP_MID_SET_TEXT 801601,'Waiting',30,40,null,null,0
exec SP_MID_SET_TEXT 801602,'Running',30,40,null,null,0
exec SP_MID_SET_TEXT 801603,'On Hold',30,40,null,null,0
exec SP_MID_SET_TEXT 801604,'Completed',30,40,null,null,0
exec SP_MID_SET_TEXT 801605,'Cancelled',30,40,null,null,0
exec SP_MID_SET_TEXT 801606,'Executed',30,40,null,null,0
exec SP_MID_SET_TEXT 801607,'Failed Condition',30,40,null,null,0
exec SP_MID_SET_TEXT 801608,'In Error',30,40,null,null,0
exec SP_MID_SET_TEXT 801609,'Unexpected Termination',30,40,null,null,0
/* Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler */
exec SP_MID_SET_TEXT 801610,'Queued',30,40,null,null,0
/* Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler */

/* eScheduleByType */
exec SP_MID_SET_TEXT 801620,'Once',30,41,null,null,0
exec SP_MID_SET_TEXT 801621,'By Day',30,41,null,null,0
exec SP_MID_SET_TEXT 801622,'By Week',30,41,null,null,0
exec SP_MID_SET_TEXT 801623,'By Month',30,41,null,null,0

/* eScheduleByMonthWeekType */
exec SP_MID_SET_TEXT 801640,'None',30,42,null,null,0
exec SP_MID_SET_TEXT 801641,'Every',30,42,null,null,0
exec SP_MID_SET_TEXT 801642,'First',30,42,null,null,0
exec SP_MID_SET_TEXT 801643,'Second',30,42,null,null,0
exec SP_MID_SET_TEXT 801644,'Third',30,42,null,null,0
exec SP_MID_SET_TEXT 801645,'Fourth',30,42,null,null,0
exec SP_MID_SET_TEXT 801646,'Last',30,42,null,null,0

/* eScheduleRepeatIntervalType */
exec SP_MID_SET_TEXT 801660,'None',30,43,null,null,0
exec SP_MID_SET_TEXT 801661,'Seconds',30,43,null,null,0
exec SP_MID_SET_TEXT 801662,'Minutes',30,43,null,null,0
exec SP_MID_SET_TEXT 801663,'Hours',30,43,null,null,0

/* eScheduleConditionType */
exec SP_MID_SET_TEXT 801680,'None',30,44,null,null,0
exec SP_MID_SET_TEXT 801681,'By File Extension',30,44,null,null,0

/* eWMexpNodeType */
/* 
exec SP_MID_SET_TEXT 802000,'Forecast',50,26,14,0,0
exec SP_MID_SET_TEXT 802001,'Allocation',50,26,14,0,0
exec SP_MID_SET_TEXT 802002,'Workflows',50,26,14,0,0
exec SP_MID_SET_TEXT 802003,'Methods',50,26,14,0,0
exec SP_MID_SET_TEXT 802004,'OTS Forecast Method',50,26,14,0,0
exec SP_MID_SET_TEXT 802005,'Forecast Balance Method',50,26,14,0,0
exec SP_MID_SET_TEXT 802006,'General Allocation Method',50,26,14,0,0
exec SP_MID_SET_TEXT 802007,'Allocation Override Method',50,26,14,0,0
exec SP_MID_SET_TEXT 802008,'Velocity Method',50,26,14,0,0
exec SP_MID_SET_TEXT 802009,'Basis Allocation Method',50,26,14,0,0
exec SP_MID_SET_TEXT 802010,'Size Allocation Method',50,26,14,0,0
exec SP_MID_SET_TEXT 802011,'Global',50,26,14,0,0
exec SP_MID_SET_TEXT 802012,'User',50,26,14,0,0
*/

/* eFilterDateType */
exec SP_MID_SET_TEXT 801690,'None',30,65,null,null,0
exec SP_MID_SET_TEXT 801691,'Today',30,65,null,null,0
exec SP_MID_SET_TEXT 801692,'Between',30,65,null,null,0
exec SP_MID_SET_TEXT 801693,'Specify dates',30,65,null,null,0
exec SP_MID_SET_TEXT 801694,'All',30,65,null,null,0

/*Begin Track #4010 - JScott - Add all forecast methods and workflows to the scheduler. */
/* eFilterDateType */
exec SP_MID_SET_TEXT 801700,'None',30,0,null,null,0
exec SP_MID_SET_TEXT 801701,'Method',30,0,null,null,0
exec SP_MID_SET_TEXT 801702,'Workflow',30,0,null,null,0

/*End Track #4010 - JScott - Add all forecast methods and workflows to the scheduler. */

/*Begin TT#391 - stodd - adding In Stock Sales to global options */
/* eGenerateSizeCodeUsing */
exec SP_MID_SET_TEXT 801720,'Sales',30,90,null,null,0
exec SP_MID_SET_TEXT 801721,'In Stock Sales',30,90,null,null,0
/*End TT#391 - stodd - adding In Stock Sales to global options */

/* eMethodType */
exec SP_MID_SET_TEXT 802100,'OTS Forecast',30,10,null,null,0
exec SP_MID_SET_TEXT 802101,'Matrix Balance',30,10,null,null,0
exec SP_MID_SET_TEXT 802102,'General Allocation',30,10,null,null,0
exec SP_MID_SET_TEXT 802103,'Allocation Override',30,10,null,null,0
exec SP_MID_SET_TEXT 802104,'Velocity',30,10,null,null,0
exec SP_MID_SET_TEXT 802105,'Rule',30,10,null,null,0
exec SP_MID_SET_TEXT 802106,'Fill Sizes',30,10,null,null,0
exec SP_MID_SET_TEXT 802107,'Basis Size',30,10,null,null,0
exec SP_MID_SET_TEXT 802108,'Reserve Size',30,10,null,null,0
exec SP_MID_SET_TEXT 802109,'Size Need',30,10,null,null,0
exec SP_MID_SET_TEXT 802110,'Size Override',30,10,null,null,0
/* Begin TT#1398-MD - stodd - clean up allocation action list */
exec SP_MID_SET_TEXT 802111,'Need',30,10,null,null,20
exec SP_MID_SET_TEXT 802112,'Size Proportional',30,10,null,null,35
exec SP_MID_SET_TEXT 802113,'Balance Proportional',30,10,null,null,25
exec SP_MID_SET_TEXT 802114,'Balance to Reserve',30,10,null,null,30
exec SP_MID_SET_TEXT 802115,'Balance Size Proportional',30,10,null,null,45
exec SP_MID_SET_TEXT 802116,'Balance Size to Reserve',30,10,null,null,50
exec SP_MID_SET_TEXT 802117,'Charge Intransit',30,10,null,null,70
exec SP_MID_SET_TEXT 802118,'Release',30,10,null,null,75
exec SP_MID_SET_TEXT 802119,'Reset',30,10,null,null,80
exec SP_MID_SET_TEXT 802120,'Cancel Size Intransit',30,10,null,null,85
exec SP_MID_SET_TEXT 802121,'Cancel Intransit',30,10,null,null,90
exec SP_MID_SET_TEXT 802122,'Cancel Size Allocation',30,10,null,null,95
exec SP_MID_SET_TEXT 802123,'Cancel Allocation',30,10,null,null,100
exec SP_MID_SET_TEXT 802124,'Delete Header',30,10,null,null,0
exec SP_MID_SET_TEXT 802125,'Cancel Rules',30,10,null,null,0
exec SP_MID_SET_TEXT 802126,'Cancel Detail Pack Allocation',30,10,null,null,120
exec SP_MID_SET_TEXT 802127,'Charge Size Intransit',30,10,null,null,73
exec SP_MID_SET_TEXT 802128,'Forecast Spread',30,10,null,null,0
exec SP_MID_SET_TEXT 802129,'Copy Chain Forecast',30,10,null,null,0
exec SP_MID_SET_TEXT 802130,'Copy Store Forecast',30,10,null,null,0
exec SP_MID_SET_TEXT 802131,'General Assortment',30,10,null,null,0  -- assorment
exec SP_MID_SET_TEXT 802132,'Modify Sales',30,10,null,null,0
exec SP_MID_SET_TEXT 802133,'Rebuild Intransit',30,10,null,null,0 /* TT#1137 (MID Track 4351) Rebuild Intransit Utility */
/* begin MID Track 4554 Anf API Workflow Enhancemnt */
exec SP_MID_SET_TEXT 802134,'Remove API Workflow',30,10,null,null,105
exec SP_MID_SET_TEXT 802135,'Apply API Workflow',30,10,null,null,110
/* end MID Track 4554 Anf API Workflow Enhancemnt */
/* Begin Enhancement - JScott - Export Method - Part 2 */
exec SP_MID_SET_TEXT 802136,'Export',30,10,null,null,0
/* End Enhancement - JScott - Export Method - Part 2 */
/* Begin Enhancement #5004 - KJohnson - Global Unlock */
exec SP_MID_SET_TEXT 802137,'Global Unlock',30,10,null,null,0
/* End Enhancement #5004 - KJohnson - Global Unlock */
/* Begin TT#43 - MD - DOConnell - Projected Sales Enhancement */
exec SP_MID_SET_TEXT 802138,'Global Lock',30,10,null,null,0
/* End TT#43 - MD - DOConnell - Projected Sales Enhancement */
/* Begin Enhancement #5196 - KJohnson - Rollup */
exec SP_MID_SET_TEXT 802139,'Rollup',30,10,null,null,0
/* End Enhancement #5196 - KJohnson - Rollup */
/* Begin TT#155 - JSmith - Size Curve Method */
exec SP_MID_SET_TEXT 802140,'Size Curve',30,10,null,null,0
/* End TT#155 */
/* Begin TT#370 - APicchetti - Build Packs Method */
exec SP_MID_SET_TEXT 802141,'Build Packs',30,10,null,null,0 
/* End TT#370 */
exec SP_MID_SET_TEXT 802142,'Reapply Total Allocation',50,19,null,null,115 /* TT#785 - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type */
exec SP_MID_SET_TEXT 802143,'Balance Size With Contraints',30,10,null,null,55  /* TT#843 – new Size Balance by Constraints */
exec SP_MID_SET_TEXT 802144,'Balance Size Bilaterally',30,10,null,null,60  /* TT#794 – Size Balance Action for Wet Seal */
exec SP_MID_SET_TEXT 802145,'Size Proportional With Contraints',50,19,null,null,40  /* TT#1391 - Jellis - Balance Size Constraints other options*/
exec SP_MID_SET_TEXT 802146,'Group Allocation',30,10,null,null,0	/* TT#708-MD - Stodd - Group Allocation Prototype. */
exec SP_MID_SET_TEXT 802147,'Balance to VSW',30,10,null,null,65		/* TT#1334-MD - Stodd - Balance to VSW */
/* End TT#1398-MD - stodd - clean up allocation action list */

/* eAssortmentActionType */
exec SP_MID_SET_TEXT 802150,'Redo',30,62,null,null,50
/* BEGIN TT#2 - stodd - assortment */
--exec SP_MID_SET_TEXT 802151,'Remove Header',30,62,null,null,0
--exec SP_MID_SET_TEXT 802152,'Delete Assortment',30,62,null,null,0
/* BEGIN TT#2 - stodd - assortment */
exec SP_MID_SET_TEXT 802153,'Cancel Assortment',30,62,null,null,60
exec SP_MID_SET_TEXT 802154,'Spread Average',30,62,null,null,20
exec SP_MID_SET_TEXT 802155,'Index to Average',30,62,null,null,30
exec SP_MID_SET_TEXT 802156,'Create Placeholders',30,62,null,null,10
/* BEGIN TT#2 - stodd - assortment */
--exec SP_MID_SET_TEXT 802157,'Quantity',30,62,null,null,0
/* BEGIN TT#2 - stodd - assortment */
exec SP_MID_SET_TEXT 802158,'Balance Assortment',30,62,null,null,40
/* Begin TT#2 - stodd - removed/obsolete */
--exec SP_MID_SET_TEXT 802159,'Insert',30,62,null,null,0
--exec SP_MID_SET_TEXT 802160,'Size Need',30,62,null,null,0
/* End TT#2 - stodd - removed/obsolete */
/* BEGIN TT#2 - stodd - assortment */
exec SP_MID_SET_TEXT 802161,'Charge Committed',30,62,null,null,0
exec SP_MID_SET_TEXT 802162,'Cancel Committed',30,62,null,null,0
exec SP_MID_SET_TEXT 802163,'Charge Intransit',30,62,null,null,0
exec SP_MID_SET_TEXT 802164,'Cancel Intransit',30,62,null,null,0
/* BEGIN TT#2 - stodd - assortment */

exec SP_MID_SET_TEXT 802166,'DC Carton Rounding',50,19,null,null,0 /* TT#1652-MD - RMatelic - DC Carton Roundning */	 
exec SP_MID_SET_TEXT 802167,'Create Master Headers',50,19,null,null,0 /* TT#1966-MD - JSmith - DC Fulfillment */	 
exec SP_MID_SET_TEXT 802168,'DC Fulfillment',50,19,null,null,0 /* TT#1966-MD - JSmith - DC Fulfillment */	 
exec SP_MID_SET_TEXT 802169,'Planning Extract',50,19,null,null,0 /* TT#2131-MD - JSmith - Halo Integration */

/* eAllocationActionStatus */
exec SP_MID_SET_TEXT 802250,'No Action Performed',30,48,null,null,0
exec SP_MID_SET_TEXT 802251,'Action completed successfully',30,48,null,null,0
exec SP_MID_SET_TEXT 802252,'Action failed',30,48,null,null,0
exec SP_MID_SET_TEXT 802253,'Velocity Basis Error',30,48,null,null,0
exec SP_MID_SET_TEXT 802254,'Header Enqueue Failed; Open update sessions to close with no save. See Audit.',30,48,null,null,0         /* TT#241 - MD - JEllis - Header Enqueue Process */
exec SP_MID_SET_TEXT 802255,'No Resource locks for header; Open update sessions to close with no save. See Audit.',30,48,null,null,0  /* TT#241 - MD - JEllis - Header Enqueue Process */           
exec SP_MID_SET_TEXT 802256,'Linked Headers Release Approved Successfully; Release deferred until all related Linked Headers Release Approved.',30,48,null,null,0  /* TT#2671 - JEllis - Release Action Fails yet headers released */           
exec SP_MID_SET_TEXT 802257,'Linked Headers Released Successfully.',30,48,null,null,0  /* TT#2671 - JEllis - Release Action Fails yet headers released */           

/* eOTSPlanStatus */
exec SP_MID_SET_TEXT 802260,'No Action Performed',30,48,null,null,0
exec SP_MID_SET_TEXT 802261,'Action completed successfully',30,48,null,null,0
exec SP_MID_SET_TEXT 802262,'Action failed',30,48,null,null,0


/* eRuleType */
exec SP_MID_SET_TEXT 802300,'Weeks of Supply',30,49,null,null,0
exec SP_MID_SET_TEXT 802301,'Ship Up To',30,49,null,null,0
exec SP_MID_SET_TEXT 802302,'Out',30,49,null,null,0
exec SP_MID_SET_TEXT 802303,'Absolute Quantity',30,49,null,null,0
exec SP_MID_SET_TEXT 802304,'Minimum',30,49,null,null,0
exec SP_MID_SET_TEXT 802305,'Ad Minimum',30,49,null,null,0
exec SP_MID_SET_TEXT 802306,'Maximum',30,49,null,null,0
exec SP_MID_SET_TEXT 802307,'Color Minimum',30,49,null,null,0
exec SP_MID_SET_TEXT 802308,'Color Maximum',30,49,null,null,0
exec SP_MID_SET_TEXT 802309,'Forward WOS',30,49,null,null,0
exec SP_MID_SET_TEXT 802310,'Proportional',30,49,null,null,0
exec SP_MID_SET_TEXT 802311,'Exact',30,49,null,null,0
exec SP_MID_SET_TEXT 802312,'Fill',30,49,null,null,0
exec SP_MID_SET_TEXT 802313,'Size Minimum',30,49,null,null,0
exec SP_MID_SET_TEXT 802314,'Size Minimum Plus 1',30,49,null,null,0
exec SP_MID_SET_TEXT 802315,'Size Maximum',30,49,null,null,0
exec SP_MID_SET_TEXT 802316,'Size Fill Up To',30,49,null,null,0
exec SP_MID_SET_TEXT 802317,'<None>',30,49,null,null,0
exec SP_MID_SET_TEXT 802318,'Minimum Basis',30,49,null,null,0
exec SP_MID_SET_TEXT 802319,'Maximum Basis',30,49,null,null,0
exec SP_MID_SET_TEXT 802320,'Ad Minimum Basis',30,49,null,null,0


/* eQuickFilterSelectionType*/
exec SP_MID_SET_TEXT 802340,'Allocation Header',30,50,null,null,0
exec SP_MID_SET_TEXT 802341,'Merchandise Level',30,50,null,null,0
exec SP_MID_SET_TEXT 802342,'Color',30,50,null,null,0
exec SP_MID_SET_TEXT 802343,'Color Group',30,50,null,null,0  
exec SP_MID_SET_TEXT 802344,'Size',30,50,null,null,0  
exec SP_MID_SET_TEXT 802345,'Date',30,50,null,null,0
exec SP_MID_SET_TEXT 802346,'Component',30,50,null,null,0
exec SP_MID_SET_TEXT 802347,'Variable',30,50,null,null,0
 
/* eFilterComparisonType */
exec SP_MID_SET_TEXT 802360,'Equal To',30,50,null,null,0
exec SP_MID_SET_TEXT 802361,'Less Than',30,50,null,null,0
exec SP_MID_SET_TEXT 802362,'Greater Than',30,50,null,null,0
exec SP_MID_SET_TEXT 802363,'Less Than or Equal To',30,50,null,null,0  
exec SP_MID_SET_TEXT 802364,'Greater Than or Equal To',30,50,null,null,0  
exec SP_MID_SET_TEXT 802365,'Not Equal To',30,50,null,null,0
exec SP_MID_SET_TEXT 802366,'Not Less Than',30,50,null,null,0
exec SP_MID_SET_TEXT 802367,'Not Greater Than',30,50,null,null,0
exec SP_MID_SET_TEXT 802368,'Not Less Than or Equal To',30,50,null,null,0
exec SP_MID_SET_TEXT 802369,'Not Greater Than or Equal To',30,50,null,null,0


/*BEGIN TT#246-MD - AGallagher – VSW Size */ 
/* eVSWSizeConstraintType */
exec SP_MID_SET_TEXT 802370,'None',30,99,null,null,0
exec SP_MID_SET_TEXT 802371,'In Store Size Minimum',30,99,null,null,0
exec SP_MID_SET_TEXT 802372,'Item Max Ideal Size',30,99,null,null,0
exec SP_MID_SET_TEXT 802373,'Combined Ideal Size Minimum',30,99,null,null,0
/*END TT#246-MD - AGallagher – VSW Size */ 

 
/* eRuleMethod */
exec SP_MID_SET_TEXT 802400,'<None>',30,38,null,null,0
exec SP_MID_SET_TEXT 802401,'Out',30,38,null,null,0
exec SP_MID_SET_TEXT 802402,'Quantity',30,38,null,null,0
exec SP_MID_SET_TEXT 802403,'Allocation Minimum',30,38,null,null,0
exec SP_MID_SET_TEXT 802404,'Allocation Maximum',30,38,null,null,0
exec SP_MID_SET_TEXT 802405,'Color Minimum',30,38,null,null,0
exec SP_MID_SET_TEXT 802406,'Color Maximum',30,38,null,null,0
exec SP_MID_SET_TEXT 802407,'Ad Minimum',30,38,null,null,0
exec SP_MID_SET_TEXT 802408,'Exact',30,38,null,null,0
exec SP_MID_SET_TEXT 802409,'Fill',30,38,null,null,0
exec SP_MID_SET_TEXT 802410,'Proportional',30,38,null,null,0

/* eModifySalesRuleType */
exec SP_MID_SET_TEXT 802430,'<None>',30,0,null,null,0
exec SP_MID_SET_TEXT 802431,'Sales Modifier',30,0,null,null,0
exec SP_MID_SET_TEXT 802432,'Sales Index',30,0,null,null,0
exec SP_MID_SET_TEXT 802433,'Plug Sales',30,0,null,null,0
exec SP_MID_SET_TEXT 802434,'S/S',30,0,null,null,0
exec SP_MID_SET_TEXT 802435,'S/S Index',30,0,null,null,0
exec SP_MID_SET_TEXT 802436,'S/S Min',30,0,null,null,0
exec SP_MID_SET_TEXT 802437,'S/S Max',30,0,null,null,0

/* eFillSizeHolesSort */
exec SP_MID_SET_TEXT 802600,'Ascending',30,53,null,null,0
exec SP_MID_SET_TEXT 802601,'Descending',30,53,null,null,0

/* eSizeBasisSort */
exec SP_MID_SET_TEXT 802610,'Ascending',30,56,null,null,0
exec SP_MID_SET_TEXT 802611,'Descending',30,56,null,null,0

/* eEquateOverrideSizeType */
exec SP_MID_SET_TEXT 802690,'Dimension',30,60,null,null,0
exec SP_MID_SET_TEXT 802691,'Size',30,60,null,null,0
exec SP_MID_SET_TEXT 802692,'Dimension/Size',30,60,null,null,0

/* eHeaderAllocationStatus */
exec SP_MID_SET_TEXT 802700,'Received Out Of Balance',30,45,null,null,0
exec SP_MID_SET_TEXT 802701,'Received In Balance',30,45,null,null,0
exec SP_MID_SET_TEXT 802702,'In use by Multi-Header',30,45,null,null,0
exec SP_MID_SET_TEXT 802703,'Pre-Size Out of Balance',30,45,null,null,0
exec SP_MID_SET_TEXT 802704,'Pre-Size In Balance',30,45,null,null,0
exec SP_MID_SET_TEXT 802705,'Allocated Out of Balance',30,45,null,null,0
exec SP_MID_SET_TEXT 802706,'Allocated In Balance',30,45,null,null,0
exec SP_MID_SET_TEXT 802707,'Sizes Out of Balance',30,45,null,null,0
exec SP_MID_SET_TEXT 802708,'All In balance',30,45,null,null,0
exec SP_MID_SET_TEXT 802709,'Released',30,45,null,null,0
exec SP_MID_SET_TEXT 802710,'Release Approved',30,45,null,null,0
exec SP_MID_SET_TEXT 802711,'Allocation Started',30,45,null,null,0  /* MID Track 3811, 3827 Status problems */

/* eHeaderShipStatus */
exec SP_MID_SET_TEXT 802730,'Not Shipped',30,46,null,null,0
exec SP_MID_SET_TEXT 802731,'Partial',30,46,null,null,0
exec SP_MID_SET_TEXT 802732,'On Hold',30,46,null,null,0
exec SP_MID_SET_TEXT 802733,'Shipped',30,46,null,null,0

/* eHeaderIntransitStatus */
exec SP_MID_SET_TEXT 802750,'Not Intransit',30,47,null,null,0
exec SP_MID_SET_TEXT 802751,'SKU',30,47,null,null,0
exec SP_MID_SET_TEXT 802752,'Style',30,47,null,null,0
exec SP_MID_SET_TEXT 802753,'Style and Size',30,47,null,null,0

/* eDatabaseSecurityTypes */
exec SP_MID_SET_TEXT 802800,'Allocation',50,67,null,null,0
exec SP_MID_SET_TEXT 802801,'Chain',50,67,null,null,0
exec SP_MID_SET_TEXT 802802,'Store',50,67,null,null,0

/* eSecurityOwnerType */
exec SP_MID_SET_TEXT 802820,'Group',50,68,null,null,0
exec SP_MID_SET_TEXT 802821,'User',50,68,null,null,0

/*eFringeFilterValueType*/
/*exec SP_MID_SET_TEXT 800848,'Units',30,70,null,null,0   */ /* MID Track 3619 Remove Fringe */
/*exec SP_MID_SET_TEXT 800849,'Percent',30,70,null,null,0 */ /* MID Track 3619 Remove Fringe */

/* Begin Track #4457 - JSmith - Add forecast versions */
/* eForecastBlendType */
exec SP_MID_SET_TEXT 802830,'None',30,76,null,null,0
exec SP_MID_SET_TEXT 802831,'Week',30,76,null,null,0
exec SP_MID_SET_TEXT 802832,'Month',30,76,null,null,0
/* End Track #4457 */

/* BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification */
/* eFillSizesToType */
exec SP_MID_SET_TEXT 802850,'Holes',30,77,null,null,0
exec SP_MID_SET_TEXT 802851,'Size Plan',30,77,null,null,0
exec SP_MID_SET_TEXT 802852,'Size Plan + Size Mins',30,77,null,null,0 /* TT#848-MD -jsobek -Fill to Size Plan Presentation */
/* End Track #4921 */

/* eSessionStatus */
exec SP_MID_SET_TEXT 802860,'Unknown',50,68,null,null,0
exec SP_MID_SET_TEXT 802861,'Logged In',50,68,null,null,0
exec SP_MID_SET_TEXT 802862,'Logged Out',50,68,null,null,0

/*BEGIN TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files */ 
/* eAPIFileProcessingDirection */
exec SP_MID_SET_TEXT 802870,'Default',30,100,null,null,0
exec SP_MID_SET_TEXT 802871,'FIFO',30,100,null,null,0
exec SP_MID_SET_TEXT 802872,'FILO',30,100,null,null,0
exec SP_MID_SET_TEXT 802873,'Use Config Setting',30,100,null,null,0
/*END TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files */ 

/* Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only */
/* eAssortmentDisplayType */
exec SP_MID_SET_TEXT 802880,'Buy',30,103,null,null,0
exec SP_MID_SET_TEXT 802881,'Post Receipt',30,103,null,null,0
/* End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only */

/* 803000 - 803999 Used by Allocation View Selection */
exec SP_MID_SET_TEXT 803001,'Header',30,33,12,0,0
exec SP_MID_SET_TEXT 803002,'Component',30,33,12,0,0

exec SP_MID_SET_TEXT 803021,'Header',30,34,12,0,0
exec SP_MID_SET_TEXT 803022,'Color',30,34,12,0,0

exec SP_MID_SET_TEXT 803031,'Variable',30,58,12,0,0
exec SP_MID_SET_TEXT 803032,'Size',30,58,12,0,0

exec SP_MID_SET_TEXT 803041,'Attribute',30,35,12,0,0
exec SP_MID_SET_TEXT 803042,'Store Grade',30,35,12,0,0

/*Begin Assortment Planning - JScott - Assortment Planning Changes */
exec SP_MID_SET_TEXT 803051,'Attribute',30,61,12,0,0
exec SP_MID_SET_TEXT 803052,'Store Grade',30,61,12,0,0

/*End Assortment Planning - JScott - Assortment Planning Changes */
exec SP_MID_SET_TEXT 803201,'Time Period Begin must not be later than Time Period end',250,0,6,null,0
exec SP_MID_SET_TEXT 803202,'Time Period Begin must not be later than Time Period end',250,0,6,null,0

/* eStoreStatus */
exec SP_MID_SET_TEXT 804000,'New',30,54,null,null,0
exec SP_MID_SET_TEXT 804001,'Comp',30,54,null,null,0
exec SP_MID_SET_TEXT 804002,'NonComp',30,54,null,null,0
exec SP_MID_SET_TEXT 804003,'Closed',30,54,null,null,0
exec SP_MID_SET_TEXT 804004,'Preopen',30,54,null,null,0


/* eAllocationWaferVariables 804300-804599 */
/* begin tt#152 - velocity balance - apicchetti */
exec SP_MID_SET_TEXT 804300,'Velocity Initial Rule Qty',30,55,null,null,0
exec SP_MID_SET_TEXT 804301,'Velocity Initial Rule Type',30,55,null,null,0
exec SP_MID_SET_TEXT 804302,'Velocity Initial Will Ship',30,55,null,null,0
/* end tt#152 - velocity balance - apicchetti */

exec SP_MID_SET_TEXT 804303,'Avg WOS',30,55,null,null,0  /*TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66) */
exec SP_MID_SET_TEXT 804304,'Velocity Rule Type Qty',30,55,null,null,0  /*TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  */
exec SP_MID_SET_TEXT 804305,'Velocity Initial Rule Type Qty',30,55,null,null,0  /*TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  */

exec SP_MID_SET_TEXT 804306,'Basis Grade',30,55,null,null,0   /* TT#638 - RMatelic - Style Review - Add Basis Variables */

exec SP_MID_SET_TEXT 804307,'Store Qty',30,55,null,null,0  /* TT#1401 - AGallagher - Reservation Stores */
exec SP_MID_SET_TEXT 804308,'VSW Qty',30,55,null,null,0  /* TT#1401 - AGallagher - Reservation Stores */
exec SP_MID_SET_TEXT 804309,'Item Max',30,55,null,null,0  /* TT#1401 - AGallagher - Reservation Stores */
exec SP_MID_SET_TEXT 804310,'VSW On Hand',30,55,null,null,0  /* TT#1401 - AGallagher - Reservation Stores */
exec SP_MID_SET_TEXT 804311,'Fields are only selectable when Velocity Grade is active',30,55,null,null,0  /* TT#1401 - AGallagher - Reservation Stores */
exec SP_MID_SET_TEXT 804312,'Item Max Override',30,55,null,null,0  /* TT#1401 - AGallagher - Reservation Stores */
exec SP_MID_SET_TEXT 804313,'VSW Size On Hand',30,55,null,null,0  /* TT#1401 - AGallagher - VSW */
exec SP_MID_SET_TEXT 804314,'Grid items are not selectable when Do Not Apply VSW is selected',30,55,null,null,0 /* TT#1401 - GTaylor - VSW */
exec SP_MID_SET_TEXT 804315,'Checking this box will not allow the VSW values to be applied',30,55,null,null,0 /* TT#1401 - GTaylor - VSW */

/* BEGIN TT#2015 - gtaylor - apply changes to lower levels */
exec SP_MID_SET_TEXT 804316,'Action Completed Successfully.',30,1,0,null,0
exec SP_MID_SET_TEXT 804317,'No Changes have been made.  Apply Changes to Lower Levels was not executed.',75,1,0,null,0
exec SP_MID_SET_TEXT 804318,'Lock Attempt Failed.  Apply Changes to Lower Levels was not executed.',70,1,0,null,0
exec SP_MID_SET_TEXT 804319,'No Low Level Processing Is Required.',40,1,0,null,0
exec SP_MID_SET_TEXT 804320,'Default Action',15,1,0,null,0
/* END TT#2015 - gtaylor - apply changes to lower levels */
exec SP_MID_SET_TEXT 804321, 'Basis VSW OnHand',30,55,null,null,0;   /* TT#4262-VStuart-Velocity-VSW On Hand */ 

exec SP_MID_SET_TEXT 804500,'None',30,55,null,null,0
exec SP_MID_SET_TEXT 804501,'Original Units Allocated',30,55,null,null,0
exec SP_MID_SET_TEXT 804502,'Qty Allocated',30,55,null,null,0
exec SP_MID_SET_TEXT 804503,'Need',30,55,null,null,0
exec SP_MID_SET_TEXT 804504,'%',30,55,null,null,0
exec SP_MID_SET_TEXT 804505,'On Hand',30,55,null,null,0
exec SP_MID_SET_TEXT 804506,'Forecast Sales',30,55,null,null,0
exec SP_MID_SET_TEXT 804507,'Forecast Stock',30,55,null,null,0
exec SP_MID_SET_TEXT 804508,'Intransit',30,55,null,null,0
exec SP_MID_SET_TEXT 804509,'Grade',30,55,null,null,0
exec SP_MID_SET_TEXT 804510,'Rule',30,55,null,null,0
exec SP_MID_SET_TEXT 804511,'Rule Qty',30,55,null,null,0
exec SP_MID_SET_TEXT 804512,'Open to Ship',30,55,null,null,0
exec SP_MID_SET_TEXT 804513,'OTS Variance',30,55,null,null,0
exec SP_MID_SET_TEXT 804514,'Received',30,55,null,null,0
exec SP_MID_SET_TEXT 804515,'# Stores',30,55,null,null,0
exec SP_MID_SET_TEXT 804516,'Avg',30,55,null,null,0
exec SP_MID_SET_TEXT 804517,'Balance',30,55,null,null,0
exec SP_MID_SET_TEXT 804518,'Total',30,55,null,null,0
exec SP_MID_SET_TEXT 804519,'Avg Weekly Sales',30,55,null,null,0
exec SP_MID_SET_TEXT 804520,'Avg Weekly Stock',30,55,null,null,0
exec SP_MID_SET_TEXT 804521,'Basis Intransit',30,55,null,null,0
exec SP_MID_SET_TEXT 804522,'Basis On Hand',30,55,null,null,0
exec SP_MID_SET_TEXT 804523,'Basis Sales',30,55,null,null,0
exec SP_MID_SET_TEXT 804524,'Basis Stock',30,55,null,null,0
exec SP_MID_SET_TEXT 804525,'Pct SellThru',30,55,null,null,0
exec SP_MID_SET_TEXT 804526,'Velocity Grade',30,55,null,null,0
exec SP_MID_SET_TEXT 804527,'Velocity Rule Type',30,55,null,null,0
exec SP_MID_SET_TEXT 804528,'Velocity Rule Qty',30,55,null,null,0
exec SP_MID_SET_TEXT 804529,'Velocity Result',30,55,null,null,0
exec SP_MID_SET_TEXT 804530,'Pct SellThru Index',30,55,null,null,0
exec SP_MID_SET_TEXT 804531,'Header On Hand',30,55,null,null,0
exec SP_MID_SET_TEXT 804532,'Header Intransit',30,55,null,null,0
exec SP_MID_SET_TEXT 804533,'Transfer',30,55,null,null,0
exec SP_MID_SET_TEXT 804534,'Size Total',30,55,null,null,0
exec SP_MID_SET_TEXT 804535,'Size Intransit',30,55,null,null,0
exec SP_MID_SET_TEXT 804536,'Size On Hand',30,55,null,null,0
exec SP_MID_SET_TEXT 804537,'Size OnHand+IT',30,55,null,null,0
exec SP_MID_SET_TEXT 804538,'Size Plan',30,55,null,null,0
exec SP_MID_SET_TEXT 804539,'Size Curve Pct',30,55,null,null,0
exec SP_MID_SET_TEXT 804540,'Size Need',30,55,null,null,0
exec SP_MID_SET_TEXT 804541,'Size Pos Need',30,55,null,null,0
exec SP_MID_SET_TEXT 804542,'Size Pct Need',30,55,null,null,0
exec SP_MID_SET_TEXT 804543,'Size Pos Pct Need',30,55,null,null,0
exec SP_MID_SET_TEXT 804544,'% to Total',30,55,null,null,0
exec SP_MID_SET_TEXT 804545,'Current WTD Sales',30,55,null,null,0
exec SP_MID_SET_TEXT 804546,'Size Sales',30,55,null,null,0
exec SP_MID_SET_TEXT 804547,'Curve Adjd Size OnHand',30,55,null,null,0
exec SP_MID_SET_TEXT 804548,'Curve Adjd Size Intransit',30,55,null,null,0
exec SP_MID_SET_TEXT 804549,'Curve Adjd Size OnHand+IT',30,55,null,null,0
exec SP_MID_SET_TEXT 804550,'Curve Adjd Size Need',30,55,null,null,0
exec SP_MID_SET_TEXT 804551,'Curve Adjd Size %Need',30,55,null,null,0
exec SP_MID_SET_TEXT 804552,'Curve Adjd Size Pos Need',30,55,null,null,0
exec SP_MID_SET_TEXT 804553,'Curve Adjd Size Pos %Need',30,55,null,null,0
exec SP_MID_SET_TEXT 804554,'Curve Adjd Size Plan',30,55,null,null,0
exec SP_MID_SET_TEXT 804555,'Curve Adjd Size Curve Pct',30,55,null,null,0
exec SP_MID_SET_TEXT 804556,'Ship Day',30,55,null,null,0 /* MID Track 3880 Add Ship To Day variable to Size and Style Review */
exec SP_MID_SET_TEXT 804557,'Need Day',30,55,null,null,0 /* MID Track 4291 Add Fill variables to Size Review */
/*  begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement */
exec SP_MID_SET_TEXT 804558,'Fill Own Plan',30,55,null,null,0 /* MID Track 4291 Add Fill variables to Size Review */
exec SP_MID_SET_TEXT 804559,'Fill Own Need',30,55,null,null,0 /* MID Track 4291 Add Fill variables to Size Review */
exec SP_MID_SET_TEXT 804560,'Fill Own %Need',30,55,null,null,0 /* MID Track 4291 Add Fill variables to Size Review */
/* end MID Track 4921 Anf#666 Fill to Size Plan Enhancement */
exec SP_MID_SET_TEXT 804561,'PreSize Allocated',30,55,null,null,0 /* MID Track 4282 Velocity overlays Fill Size Holes Allocation */
exec SP_MID_SET_TEXT 804562,'Fill Fwd Sales',30,55,null,null,0 /* MID Track 4921 AnF#666 Fill to Size Plan Enhancement */
exec SP_MID_SET_TEXT 804563,'Fill Fwd Stock',30,55,null,null,0 /* MID Track 4921 AnF#666 Fill to Size Plan Enhancement */
exec SP_MID_SET_TEXT 804564,'Fill Fwd Plan',30,55,null,null,0 /* MID Track 4921 AnF#666 Fill to Size Plan Enhancement */
exec SP_MID_SET_TEXT 804565,'Fill Fwd Need',30,55,null,null,0 /* MID Track 4921 AnF#666 Fill to Size Plan Enhancement */
exec SP_MID_SET_TEXT 804566,'Fill Fwd %Need',30,55,null,null,0 /* MID Track 4921 AnF#666 Fill to Size Plan Enhancement */
exec SP_MID_SET_TEXT 804567,'Sort Seq',30,55,null,null,0 /*MID Track 6079 Qty Allocated change not accepted after Sort */
/* begin TT#59 Implement Store Temp Locks  */
exec SP_MID_SET_TEXT 804568,'Has Ship Priority',30,55,null,null,0
exec SP_MID_SET_TEXT 804569,'Available Capacity',30,55,null,null,0
exec SP_MID_SET_TEXT 804570,'Capacity Exceed %',30,55,null,null,0
exec SP_MID_SET_TEXT 804571,'Capacity Maximum',30,55,null,null,0
exec SP_MID_SET_TEXT 804572,'Capacity Reached',30,55,null,null,0
exec SP_MID_SET_TEXT 804573,'May Exceed Capacity',30,55,null,null,0
exec SP_MID_SET_TEXT 804574,'May Exceed Maximum',30,55,null,null,0
exec SP_MID_SET_TEXT 804575,'Used Capacity',30,55,null,null,0
exec SP_MID_SET_TEXT 804576,'%Need Limit Reached',30,55,null,null,0
exec SP_MID_SET_TEXT 804577,'Aloctn From BottomUpSize',30,55,null,null,0
exec SP_MID_SET_TEXT 804578,'Aloctn From Size Breakout',30,55,null,null,0
exec SP_MID_SET_TEXT 804579,'Aloctn From Pack Need',30,55,null,null,0
exec SP_MID_SET_TEXT 804580,'Aloctn Chgd After Multi Split',30,55,null,null,0
exec SP_MID_SET_TEXT 804581,'Color Maximum',30,55,null,null,0
exec SP_MID_SET_TEXT 804582,'Color Minimum',30,55,null,null,0
exec SP_MID_SET_TEXT 804583,'Filled Size Holes',30,55,null,null,0
exec SP_MID_SET_TEXT 804584,'Had Need',30,55,null,null,0
exec SP_MID_SET_TEXT 804585,'Manually Allocated',30,55,null,null,0
exec SP_MID_SET_TEXT 804586,'Maximum',30,55,null,null,0
exec SP_MID_SET_TEXT 804587,'Minimum',30,55,null,null,0
exec SP_MID_SET_TEXT 804588,'Rule Aloctn From Child',30,55,null,null,0
exec SP_MID_SET_TEXT 804589,'Rule Aloctn From Chosen Rule',30,55,null,null,0
exec SP_MID_SET_TEXT 804590,'Rule Aloctn From Parent',30,55,null,null,0
exec SP_MID_SET_TEXT 804591,'Shipping Status',30,55,null,null,0
exec SP_MID_SET_TEXT 804592,'Unit Need Before',30,55,null,null,0
exec SP_MID_SET_TEXT 804593,'Was Auto Allocated',30,55,null,null,0
exec SP_MID_SET_TEXT 804594,'Qty Auto Aloctd',30,55,null,null,0
exec SP_MID_SET_TEXT 804595,'Qty Aloctd by Rule',30,55,null,null,0
exec SP_MID_SET_TEXT 804596,'Qty Shipped',30,55,null,null,0
exec SP_MID_SET_TEXT 804597,'%Need Before',30,55,null,null,0
exec SP_MID_SET_TEXT 804598,'Assortment Grade',30,55,null,null,0  /* TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working */
/* end TT#59 Implement Store Temp Locks  */
exec SP_MID_SET_TEXT 804599,'Committed',30,55,null,null,0		/* TT#1224 - stodd */


/*Begin Assortment Planning - JScott - Assortment Planning Changes */
/* eAssortmentHeadingVariables 804600-804620 */
exec SP_MID_SET_TEXT 804600,'Assortment',30,78,null,null,0
exec SP_MID_SET_TEXT 804601,'Plan Level',30,78,null,null,0
exec SP_MID_SET_TEXT 804602,'Placeholder Style/SKU',30,78,null,null,0
exec SP_MID_SET_TEXT 804603,'Description',30,78,null,null,0
exec SP_MID_SET_TEXT 804604,'Header ID',30,78,null,null,0
exec SP_MID_SET_TEXT 804605,'Color',30,78,null,null,0
exec SP_MID_SET_TEXT 804606,'Pack',30,78,null,null,0
exec SP_MID_SET_TEXT 804607,'Style/SKU',30,78,null,null,0
/* Begin TT#1227 - stodd - sort seq */
exec SP_MID_SET_TEXT 804608,'Placeholder Seq',30,78,null,null,0
exec SP_MID_SET_TEXT 804609,'Header Seq',30,78,null,null,0
/* Begin TT#1227 - stodd - sort seq */

/* eAssortmentTotalVariables 804620-804650 */
exec SP_MID_SET_TEXT 804620,'Total %',30,79,null,null,0
exec SP_MID_SET_TEXT 804621,'Total Units',30,79,null,null,0
exec SP_MID_SET_TEXT 804622,'Header Units',30,79,null,null,0
exec SP_MID_SET_TEXT 804623,'Reserve Units',30,79,null,null,0
exec SP_MID_SET_TEXT 804624,'Average Units',30,79,null,null,0
exec SP_MID_SET_TEXT 804625,'Total Retail',30,79,null,null,0
exec SP_MID_SET_TEXT 804626,'Total Cost',30,79,null,null,0
exec SP_MID_SET_TEXT 804627,'Unit Retail',30,79,null,null,0
exec SP_MID_SET_TEXT 804628,'Unit Cost',30,79,null,null,0
exec SP_MID_SET_TEXT 804629,'MU %',30,79,null,null,0
exec SP_MID_SET_TEXT 804630,'Balance',30,79,null,null,0
exec SP_MID_SET_TEXT 804631,'On Hand',30,79,null,null,0
exec SP_MID_SET_TEXT 804632,'Intransit',30,79,null,null,0
exec SP_MID_SET_TEXT 804633,'Multiple',30,79,null,null,0
exec SP_MID_SET_TEXT 804634,'Minimum',30,79,null,null,0
exec SP_MID_SET_TEXT 804635,'Maximum',30,79,null,null,0
exec SP_MID_SET_TEXT 804636,'Header Units',30,77,null,null,0
exec SP_MID_SET_TEXT 804637,'Committed',30,77,null,null,0	/* TT#1224 - stodd - add committed */
exec SP_MID_SET_TEXT 804638,'# Stores Alloc',30,77,null,null,0	/* TT#4294 - stodd - Average Units in Matrix Enahancement */

/* eAssortmentDetailVariables 804650-804680 */
exec SP_MID_SET_TEXT 804650,'Total %',30,80,null,null,0
exec SP_MID_SET_TEXT 804651,'Total Units',30,80,null,null,0
exec SP_MID_SET_TEXT 804653,'Total Units % To Set',30,80,null,null,0
exec SP_MID_SET_TEXT 804654,'Total Units % To All',30,80,null,null,0
exec SP_MID_SET_TEXT 804655,'Average Units',30,80,null,null,0
exec SP_MID_SET_TEXT 804656,'Index',30,80,null,null,0
exec SP_MID_SET_TEXT 804657,'Unit Retail',30,80,null,null,0
exec SP_MID_SET_TEXT 804658,'Unit Cost',30,80,null,null,0
exec SP_MID_SET_TEXT 804659,'# Stores Alloc',30,80,null,null,0	/* TT#4294 - stodd - Average Units in Matrix Enahancement */

/* eAssortmentSummaryVariables 804680-804700 */
exec SP_MID_SET_TEXT 804680,'Units',30,81,null,null,0
exec SP_MID_SET_TEXT 804681,'# Stores',30,81,null,null,0
exec SP_MID_SET_TEXT 804682,'Average Store',30,81,null,null,0
exec SP_MID_SET_TEXT 804683,'Average Units',30,81,null,null,0
exec SP_MID_SET_TEXT 804684,'Index',30,81,null,null,0
exec SP_MID_SET_TEXT 804685,'Basis',30,81,null,null,0
exec SP_MID_SET_TEXT 804686,'Need',30,81,null,null,0
exec SP_MID_SET_TEXT 804687,'% Need',30,81,null,null,0
exec SP_MID_SET_TEXT 804688,'Intransit',30,81,null,null,0
exec SP_MID_SET_TEXT 804689,'Committed',30,81,null,null,0	/* TT#1224 - stodd - add committed */
exec SP_MID_SET_TEXT 804690,'OnHand',30,81,null,null,0		/* TT#845-MD - stodd - add OnHand to Summary */
exec SP_MID_SET_TEXT 804691,'Total VSW OnHand',30,81,null,null,0	/* TT#952 - MD - stodd - add matrix to Group Allocation Review */
exec SP_MID_SET_TEXT 804692,'Balance',30,81,null,null,0			/* TT#3817 - stodd - Saving View with Balance checked - */
/*End Assortment Planning - JScott - Assortment Planning Changes */

/* begin TT#173  Provide database container for large data collections */
/* eAllocationDatabaseStoreVariables 805101-805200 */
exec SP_MID_SET_TEXT 805101,'DETAIL_AUDIT_FLAGS',30,86,null,null,0
exec SP_MID_SET_TEXT 805102,'MINIMUM',30,86,null,null,0
exec SP_MID_SET_TEXT 805103,'MAXIMUM',30,86,null,null,0
exec SP_MID_SET_TEXT 805104,'PRIMARY_MAXIMUM',30,86,null,null,0
exec SP_MID_SET_TEXT 805105,'UNITS_ALLOCATED',30,86,null,null,0
exec SP_MID_SET_TEXT 805106,'PACKS_ALLOCATED',30,86,null,null,0
exec SP_MID_SET_TEXT 805107,'UNITS_ALLOCATED_BY_AUTO',30,86,null,null,0
exec SP_MID_SET_TEXT 805108,'PACKS_ALLOCATED_BY_AUTO',30,86,null,null,0
exec SP_MID_SET_TEXT 805109,'UNITS_ALLOCATED_BY_RULE',30,86,null,null,0
exec SP_MID_SET_TEXT 805110,'PACKS_ALLOCATED_BY_RULE',30,86,null,null,0
exec SP_MID_SET_TEXT 805111,'SHIP_STATUS_FLAGS',30,86,null,null,0
exec SP_MID_SET_TEXT 805112,'UNITS_SHIPPED',30,86,null,null,0
exec SP_MID_SET_TEXT 805113,'PACKS_SHIPPED',30,86,null,null,0
exec SP_MID_SET_TEXT 805114,'CHOSEN_RULE_TYPE',30,86,null,null,0
exec SP_MID_SET_TEXT 805115,'CHOSEN_RULE_LAYER',30,86,null,null,0
exec SP_MID_SET_TEXT 805116,'CHOSEN_RULE_UNITS',30,86,null,null,0
exec SP_MID_SET_TEXT 805117,'CHOSEN_RULE_PACKS',30,86,null,null,0
exec SP_MID_SET_TEXT 805118,'NEED_DAY',30,86,null,null,0
exec SP_MID_SET_TEXT 805119,'UNIT_NEED_BEFORE',30,86,null,null,0
exec SP_MID_SET_TEXT 805120,'UNIT_PLAN_BEFORE',30,86,null,null,0
exec SP_MID_SET_TEXT 805121,'GENERAL_AUDIT_FLAGS',30,86,null,null,0
exec SP_MID_SET_TEXT 805122,'GRADE_INDEX',30,86,null,null,0
exec SP_MID_SET_TEXT 805123,'SHIP_TO_DAY',30,86,null,null,0
exec SP_MID_SET_TEXT 805124,'CAPACITY_UNITS',30,86,null,null,0
exec SP_MID_SET_TEXT 805125,'CAPACITY_EXCEED_BY_PERCENT',30,86,null,null,0
exec SP_MID_SET_TEXT 805126,'VSW REVERSE ONHAND',30,86,null,null,0 /* TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4*/

/*  eForecastBaseDatabaseStoreVariables 806101-806200 */
exec SP_MID_SET_TEXT 806101,'SALES',30,87,null,null,0
exec SP_MID_SET_TEXT 806102,'SALES_REG',30,87,null,null,0
exec SP_MID_SET_TEXT 806103,'SALES_PROMO',30,87,null,null,0
exec SP_MID_SET_TEXT 806104,'SALES_MKDN',30,87,null,null,0
exec SP_MID_SET_TEXT 806105,'STOCK',30,87,null,null,0
exec SP_MID_SET_TEXT 806106,'STOCK_REG',30,87,null,null,0
exec SP_MID_SET_TEXT 806107,'STOCK_MKDN',30,87,null,null,0
/* begin TT#391 - stodd - size day to week summary */
exec SP_MID_SET_TEXT 806108,'IN_STOCK_SALES',30,87,null,null,0
exec SP_MID_SET_TEXT 806109,'IN_STOCK_SALES_REG',30,87,null,null,0
exec SP_MID_SET_TEXT 806110,'IN_STOCK_SALES_PROMO',30,87,null,null,0
exec SP_MID_SET_TEXT 806111,'IN_STOCK_SALES_MKDN',30,87,null,null,0
exec SP_MID_SET_TEXT 806112,'ACCUM_SELL_THRU_SALES',30,87,null,null,0
exec SP_MID_SET_TEXT 806113,'ACCUM_SELL_THRU_STOCK',30,87,null,null,0
exec SP_MID_SET_TEXT 806114,'DAYS_IN_STOCK',30,87,null,null,0
exec SP_MID_SET_TEXT 806115,'RECEIVED_STOCK',30,87,null,null,0
/* End TT#391 - stodd - size day to week summary */

/* BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58) */
/* eGroupApplyMinMax (Velocity Min/Max types 806201-806225) */
exec SP_MID_SET_TEXT 806201,'None',30,1,null,null,0
exec SP_MID_SET_TEXT 806202,'Store Grade',30,1,null,null,0
exec SP_MID_SET_TEXT 806203,'Velocity Grade',30,1,null,null,0
/* END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)*/

/*  Client Custom Forecasting Database Store Variables 807100-807200 */
/*  eForecastCustomDatabaseStoreVariables 807100-807200 */
exec SP_MID_SET_TEXT 807100,'Not_A_Variable',30,88,null,null,0


/* end  TT#173  Provide database container for large data collections */



-- add security functions
/* eSecurityFunctions */
exec SP_MID_SET_TEXT 810000,'Administration',50,19,null,null,0
/*BEGIN TT#901-MD -jsobek -Batch Only Mode */
exec [dbo].[SP_MID_SET_TEXT] 810050,'System',50,19,null,null,0 
/*END TT#901-MD -jsobek -Batch Only Mode */
exec SP_MID_SET_TEXT 810100,'Calendar',50,19,null,null,0
exec SP_MID_SET_TEXT 810110,'Definition',50,19,null,null,0
exec SP_MID_SET_TEXT 810120,'Range',50,19,null,null,0
exec SP_MID_SET_TEXT 810200,'Headers',50,19,null,null,0
exec SP_MID_SET_TEXT 810210,'Characteristics',50,19,null,null,0
/*Begin Track #3785 - JScott - Remove Administration->Colors and Administration->Size->Size Definition securities. */
/*exec SP_MID_SET_TEXT 810300,'Colors',50,19,null,null,0 */
/*End Track #3785 - JScott - Remove Administration->Colors and Administration->Size->Size Definition securities. */
exec SP_MID_SET_TEXT 810400,'Global Options',50,19,null,null,0
exec SP_MID_SET_TEXT 810410,'Company Information',50,19,null,null,0
exec SP_MID_SET_TEXT 810420,'Display Options',50,19,null,null,0
exec SP_MID_SET_TEXT 810430,'OTS Plan Versions',50,19,null,null,0
exec SP_MID_SET_TEXT 810440,'Allocation Defaults',50,19,null,null,0
/*Begin Track #3784 - JScott - Add security for Header Gloabl Options */
exec SP_MID_SET_TEXT 810450,'Allocation Headers',50,19,null,null,0
/*End Track #3784 - JScott - Add security for Header Gloabl Options */
/*Begin Track #6240 - stodd - Add security for basis labels and ots defaults */
exec SP_MID_SET_TEXT 810460,'Basis Labels',50,19,null,null,0
exec SP_MID_SET_TEXT 810470,'OTS Defaults',50,19,null,null,0
/*End Track #6240 - stodd - Add security for basis labels and ots defaults */
exec SP_MID_SET_TEXT 810500,'Hierarchies',50,19,null,null,0
exec SP_MID_SET_TEXT 810510,'Alternate',50,19,null,null,0
exec SP_MID_SET_TEXT 810520,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 810525,'Properties',50,19,null,null,0
exec SP_MID_SET_TEXT 810530,'Node Properties',50,19,null,null,0
exec SP_MID_SET_TEXT 810531,'Capacity',50,19,null,null,0
exec SP_MID_SET_TEXT 810532,'Daily Percentages',50,19,null,null,0
exec SP_MID_SET_TEXT 810533,'Eligibility',50,19,null,null,0
exec SP_MID_SET_TEXT 810534,'Profile',50,19,null,null,0
exec SP_MID_SET_TEXT 810535,'Purge',50,19,null,null,0
exec SP_MID_SET_TEXT 810536,'Store Grades',50,19,null,null,0
exec SP_MID_SET_TEXT 810537,'Velocity',50,19,null,null,0
exec SP_MID_SET_TEXT 810538,'Stock Min/Max',50,19,null,null,0
exec SP_MID_SET_TEXT 810539,'Characteristics',50,19,null,null,0
exec SP_MID_SET_TEXT 810540,'Nodes',50,19,null,null,0
/* Begin TT#155 - JScott - Add Size Curve info to Node Properties */
exec SP_MID_SET_TEXT 810541,'Size Curves',50,19,null,null,0
/* End TT#155 - JScott - Add Size Curve info to Node Properties */
--Begin TT#1498 - DOConnell - Chain Plan - Set Percentages Phase 3
exec SP_MID_SET_TEXT 810542,'Chain Set Percent',50,19,null,null,0
--Begin TT#1498 - DOConnell - Chain Plan - Set Percentages Phase 3
--Begin TT1401 - GTaylor - Reservation Stores
exec SP_MID_SET_TEXT 810543,'VSW',50,19,null,null,0
--End TT#1401 - GTaylor - Reservation Stores
--Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
exec SP_MID_SET_TEXT 810544,'Size Out Of Stock',50,19,null,null,0
--End TT#483 - JScott - Add Size Lost Sales criteria and processing
exec SP_MID_SET_TEXT 810550,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 810560,'Organizational',50,19,null,null,0
exec SP_MID_SET_TEXT 810565,'Properties',50,19,null,null,0
exec SP_MID_SET_TEXT 810570,'Node Properties',50,19,null,null,0
exec SP_MID_SET_TEXT 810571,'Capacity',50,19,null,null,0
exec SP_MID_SET_TEXT 810572,'Daily Percentages',50,19,null,null,0
exec SP_MID_SET_TEXT 810573,'Eligibility',50,19,null,null,0
exec SP_MID_SET_TEXT 810574,'Profile',50,19,null,null,0
exec SP_MID_SET_TEXT 810575,'Purge',50,19,null,null,0
exec SP_MID_SET_TEXT 810576,'Store Grades',50,19,null,null,0
exec SP_MID_SET_TEXT 810577,'Velocity',50,19,null,null,0
exec SP_MID_SET_TEXT 810578,'Stock Min/Max',50,19,null,null,0
exec SP_MID_SET_TEXT 810579,'Characteristics',50,19,null,null,0
exec SP_MID_SET_TEXT 810580,'Nodes',50,19,null,null,0
/* Begin TT#155 - JScott - Add Size Curve info to Node Properties */
exec SP_MID_SET_TEXT 810581,'Size Curves',50,19,null,null,0
/* End TT#155 - JScott - Add Size Curve info to Node Properties */
--Begin TT#1498 - DOConnell - Chain Plan - Set Percentages Phase 3
exec SP_MID_SET_TEXT 810582,'Chain Set Percent',50,19,null,null,0
--Begin TT#1498 - DOConnell - Chain Plan - Set Percentages Phase 3
--Begin TT#1401 - GTaylor - Reservation Stores
exec SP_MID_SET_TEXT 810583,'VSW',50,19,null,null,0
--End TT#1401 - GTaylor - Reservation Stores
--Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
exec SP_MID_SET_TEXT 810584,'Size Out Of Stock',50,19,null,null,0
--End TT#483 - JScott - Add Size Lost Sales criteria and processing
exec SP_MID_SET_TEXT 810590,'Characteristics',50,19,null,null,0
exec SP_MID_SET_TEXT 810600,'Models',50,19,null,null,0
exec SP_MID_SET_TEXT 810610,'Eligibility',50,19,null,null,0
exec SP_MID_SET_TEXT 810620,'Sales Modifier',50,19,null,null,0
exec SP_MID_SET_TEXT 810630,'Stock Modifier',50,19,null,null,0
exec SP_MID_SET_TEXT 810640,'Forecasting',50,19,null,null,0
exec SP_MID_SET_TEXT 810650,'Matrix Balance',50,19,null,null,0
exec SP_MID_SET_TEXT 810660,'FWOS Override',50,19,null,null,0  /* TT#432-MD - JSmith - Login into new database with new user and received "Index was outside the bounds of the array" error */
--exec SP_MID_SET_TEXT 810670,'FWOS Override',50,19,null,null,0 /* TT#432-MD - JSmith - Login into new database with new user and received "Index was outside the bounds of the array" error */
exec SP_MID_SET_TEXT 810680,'FWOS Max',50,19,null,null,0  /*TT#108 - MD - DOConnell - FWOS Max Model */
exec SP_MID_SET_TEXT 810800,'Security',50,19,null,null,0
exec SP_MID_SET_TEXT 810810,'Users',50,19,null,null,0
exec SP_MID_SET_TEXT 810820,'Groups',50,19,null,null,0
exec SP_MID_SET_TEXT 810900,'Size',50,19,null,null,0
/*Begin Track #3785 - JScott - Remove Administration->Colors and Administration->Size->Size Definition securities. */
/*exec SP_MID_SET_TEXT 810910,'Definition',50,19,null,null,0 */
/*End Track #3785 - JScott - Remove Administration->Colors and Administration->Size->Size Definition securities. */
exec SP_MID_SET_TEXT 810920,'Curves',50,19,null,null,0
exec SP_MID_SET_TEXT 810930,'Groups',50,19,null,null,0
exec SP_MID_SET_TEXT 810940,'Size Constraints',50,19,null,null,0
/*exec SP_MID_SET_TEXT 810950,'Size Fringe',50,19,null,null,0   */ /* MID Track 3619 Remove Fringe */
exec SP_MID_SET_TEXT 810960,'Size Alternates',50,19,null,null,0
exec SP_MID_SET_TEXT 811000,'Store Attributes',50,19,null,null,0
/*Begin Track #4872 - JSmith - Global/User Attributes. */
exec SP_MID_SET_TEXT 811010,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 811020,'User',50,19,null,null,0
/*End Track #4872 */
exec SP_MID_SET_TEXT 811100,'Stores',50,19,null,null,0
exec SP_MID_SET_TEXT 811110,'Profiles',50,19,null,null,0
exec SP_MID_SET_TEXT 811120,'Characteristics',50,19,null,null,0
exec SP_MID_SET_TEXT 811121,'Override Low Levels',50,19,null,null,0
exec SP_MID_SET_TEXT 811122,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 811123,'Global',50,19,null,null,0


exec SP_MID_SET_TEXT 820000,'Allocation',50,19,null,null,0
exec SP_MID_SET_TEXT 821000,'Actions',50,19,null,null,0
exec SP_MID_SET_TEXT 821001,'Need',50,19,null,null,0
exec SP_MID_SET_TEXT 821002,'Balance Proportional',50,19,null,null,0
exec SP_MID_SET_TEXT 821003,'Balance to Reserve',50,19,null,null,0
exec SP_MID_SET_TEXT 821004,'Charge In-Transit',50,19,null,null,0
exec SP_MID_SET_TEXT 821005,'Release',50,19,null,null,0
exec SP_MID_SET_TEXT 821006,'Reset',50,19,null,null,0
exec SP_MID_SET_TEXT 821007,'Cancel In-Transit',50,19,null,null,0
exec SP_MID_SET_TEXT 821008,'Cancel Allocation',50,19,null,null,0
exec SP_MID_SET_TEXT 821009,'Size Proportional',50,19,null,null,0
exec SP_MID_SET_TEXT 821010,'Balance Size Proportional',50,19,null,null,0
exec SP_MID_SET_TEXT 821011,'Cancel Size In-Transit',50,19,null,null,0
exec SP_MID_SET_TEXT 821012,'Cancel Size Allocation',50,19,null,null,0
/* Begin Issue 4554 */
exec SP_MID_SET_TEXT 821014,'Cancel API Workflow',50,19,null,null,0
exec SP_MID_SET_TEXT 821015,'Apply API Workflow',50,19,null,null,0
/* End Issue 4554 */
exec SP_MID_SET_TEXT 821016,'Reapply Total Allocation',50,19,null,null,0 /* TT#785 - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type */
exec SP_MID_SET_TEXT 821017,'Balance Size With Constraints',50,19,null,null,0 /* TT#843 - New Size Constraints Balance */
exec SP_MID_SET_TEXT 821018,'Balance Size Bilaterally',50,19,null,null,0 /* TT#794 - New Size Balance for Wet Seal */
exec SP_MID_SET_TEXT 821019,'Size Proportional With Constraints',50,19,null,null,0 /* TT#1391 - JEllis - Balance Size With Constraint Other Options */
exec SP_MID_SET_TEXT 821020,'Balance Size to Reserve',50,19,null,null,0 /*  Development TT#22 - JSmith- Security -> Allocation Actions->Balance Size to Reserve */
exec SP_MID_SET_TEXT 821022,'Balance to VSW',50,19,null,null,0		/* TT#1334-MD - stodd - Balance to VSW */

exec SP_MID_SET_TEXT 822000,'Methods',50,19,null,null,0
exec SP_MID_SET_TEXT 822100,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 822101,'General Allocation',50,19,null,null,0
exec SP_MID_SET_TEXT 822102,'Allocation Override',50,19,null,null,0
exec SP_MID_SET_TEXT 822103,'Rule',50,19,null,null,0
exec SP_MID_SET_TEXT 822104,'Velocity',50,19,null,null,0
exec SP_MID_SET_TEXT 822105,'Fill Sizes',50,19,null,null,0
exec SP_MID_SET_TEXT 822106,'Basis Size',50,19,null,null,0
exec SP_MID_SET_TEXT 822107,'Size Need',50,19,null,null,0
exec SP_MID_SET_TEXT 822108,'Size Override',50,19,null,null,0
exec SP_MID_SET_TEXT 822109,'General Assortment',50,19,null,null,0
/* Begin TT#155 - JSmith - Size Curve Method */
exec SP_MID_SET_TEXT 822110,'Size Curve',50,19,null,null,0
/* End TT#155 */
exec SP_MID_SET_TEXT 822111,'Build Packs',50,19,null,null,0  /* Begin TT#825 - JSmith - Not user friendly error when logging into application with a defined user */
exec SP_MID_SET_TEXT 822112,'Group Allocation',50,19,null,null,0	/* TT#708-MD - Stodd - Group Allocation Prototype. */
exec SP_MID_SET_TEXT 822113,'DC Carton Rounding',50,19,null,null,0	/* TT#1652-MD - RMatelic - DC Carton rounding */   
exec SP_MID_SET_TEXT 822114,'Create Master Headers',50,19,null,null,0	/* TT#1966-MD - JSmith - DC Fulfillment */ 
exec SP_MID_SET_TEXT 822115,'DC Fulfillment',50,19,null,null,0	/* TT#1966-MD - JSmith - DC Fulfillment */ 
exec SP_MID_SET_TEXT 822200,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 822201,'General Allocation',50,19,null,null,0
exec SP_MID_SET_TEXT 822202,'Allocation Override',50,19,null,null,0
exec SP_MID_SET_TEXT 822203,'Rule',50,19,null,null,0
exec SP_MID_SET_TEXT 822204,'Velocity',50,19,null,null,0
exec SP_MID_SET_TEXT 822205,'Fill Sizes',50,19,null,null,0
exec SP_MID_SET_TEXT 822206,'Basis Size',50,19,null,null,0
exec SP_MID_SET_TEXT 822207,'Size Need',50,19,null,null,0
exec SP_MID_SET_TEXT 822208,'Size Override',50,19,null,null,0
exec SP_MID_SET_TEXT 822209,'General Assortment',50,19,null,null,0
/* Begin TT#155 - JSmith - Size Curve Method */
exec SP_MID_SET_TEXT 822210,'Size Curve',50,19,null,null,0
/* End TT#155 */
exec SP_MID_SET_TEXT 822211,'Build Packs',50,19,null,null,0  /* Begin TT#825 - JSmith - Not user friendly error when logging into application with a defined user */
exec SP_MID_SET_TEXT 822212,'Group Allocation',50,19,null,null,0		/* TT#708-MD - Stodd - Group Allocation Prototype. */
exec SP_MID_SET_TEXT 822213,'DC Carton Rounding',50,19,null,null,0  /* TT#1652-MD - RMatelic - DC Carton rounding */   
exec SP_MID_SET_TEXT 822214,'Create Master Headers',50,19,null,null,0  /* TT#1966-MD - JSmith - DC Fulfillment */   
exec SP_MID_SET_TEXT 822215,'DC Fulfillment',50,19,null,null,0  /* TT#1966-MD - JSmith - DC Fulfillment */   

exec SP_MID_SET_TEXT 823000,'Workflows',50,19,null,null,0
exec SP_MID_SET_TEXT 823100,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 823200,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 825000,'Headers',50,19,null,null,0
exec SP_MID_SET_TEXT 825100,'Non-Interfaced',50,19,null,null,0
exec SP_MID_SET_TEXT 825200,'Interfaced',50,19,null,null,0
/* Begin TT#254 - RMatelic - Add Header Component security to interfaced headers */
exec SP_MID_SET_TEXT 825210,'Header',50,19,null,null,0 
exec SP_MID_SET_TEXT 825220,'Pack/Bulk',50,19,null,null,0 
/* End TT#254  */
exec SP_MID_SET_TEXT 826000,'Review',50,19,null,null,0
exec SP_MID_SET_TEXT 826100,'Style',50,19,null,null,0
exec SP_MID_SET_TEXT 826200,'Summary',50,19,null,null,0
exec SP_MID_SET_TEXT 826300,'Size',50,19,null,null,0
--exec SP_MID_SET_TEXT 826400,'Assortment',50,19,null,null,0 // TT#2 - JSmith - Assortment Security

/* Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail */
exec SP_MID_SET_TEXT 827000,'Views',50,19,null,null,0
exec SP_MID_SET_TEXT 827100,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 827200,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 827110,'Workspace',50,19,null,null,0
exec SP_MID_SET_TEXT 827111,'Velocity Method',50,19,null,null,0
exec SP_MID_SET_TEXT 827112,'Velocity Store Detail',50,19,null,null,0
/* Begin TT#454 - RMatelic - -Add Views in Style Review */
exec SP_MID_SET_TEXT 827113,'Style Review',50,19,null,null,0  
/* End TT#454 */
/* Begin TT#456 - RMatelic - Add Views in Size Review */
exec SP_MID_SET_TEXT 827114,'Size Review',50,19,null,null,0 
/* End TT#456 */

exec SP_MID_SET_TEXT 827210,'Workspace',50,19,null,null,0
exec SP_MID_SET_TEXT 827211,'Velocity Method',50,19,null,null,0
exec SP_MID_SET_TEXT 827212,'Velocity Store Detail',50,19,null,null,0 
/* Begin TT#454 - RMatelic - -Add Views in Style Review */
exec SP_MID_SET_TEXT 827213,'Style Review',50,19,null,null,0  
/* End TT#454 */
/* End TT#231 */
/* Begin TT#456 - RMatelic - Add Views in Size Review */
exec SP_MID_SET_TEXT 827214,'Size Review',50,19,null,null,0  
/* End TT#456 */

exec SP_MID_SET_TEXT 830000,'OTS Forecast',50,19,null,null,0
exec SP_MID_SET_TEXT 831000,'Methods',50,19,null,null,0
exec SP_MID_SET_TEXT 831100,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 831110,'OTS Plan',50,19,null,null,0
exec SP_MID_SET_TEXT 831111,'Matrix Balance',50,19,null,null,0
exec SP_MID_SET_TEXT 831112,'Forecast Spread',50,19,null,null,0
exec SP_MID_SET_TEXT 831113,'Copy Chain Forecast',50,19,null,null,0
exec SP_MID_SET_TEXT 831114,'Copy Store Forecast',50,19,null,null,0
exec SP_MID_SET_TEXT 831115,'Modify Sales',50,19,null,null,0
/* Begin Enhancement - JScott - Export Method - Part 2 */
exec SP_MID_SET_TEXT 831116,'Export',50,19,null,null,0
/* End Enhancement - JScott - Export Method - Part 2 */
/* Begin Enhancement - KJohnson - Global Unlock */
exec SP_MID_SET_TEXT 831117,'Global Unlock',50,19,null,null,0
/* End Enhancement - KJohnson - Global Unlock */
/* Begin Enhancement - KJohnson - Multi Level Spread */
exec SP_MID_SET_TEXT 831118,'Multi Level Spread',50,19,null,null,0
/* End Enhancement - KJohnson - Multi Level Spread */
/* Begin Enhancement - KJohnson - Rollup */
exec SP_MID_SET_TEXT 831119,'Rollup',50,19,null,null,0
/* End Enhancement - KJohnson - Rollup */
exec SP_MID_SET_TEXT 831120,'Global Lock',50,19,null,null,0	/* TT#43 – MD – DOConnell – Projected Sales Enhancement */
exec SP_MID_SET_TEXT 831122,'Planning Extract',50,19,null,null,0	/* TT#2131-MD - JSmith - Halo Integration */
exec SP_MID_SET_TEXT 831200,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 831210,'OTS Plan',50,19,null,null,0
exec SP_MID_SET_TEXT 831211,'Matrix Balance',50,19,null,null,0
exec SP_MID_SET_TEXT 831212,'Forecast Spread',50,19,null,null,0
exec SP_MID_SET_TEXT 831213,'Copy Chain Forecast',50,19,null,null,0
exec SP_MID_SET_TEXT 831214,'Copy Store Forecast',50,19,null,null,0
exec SP_MID_SET_TEXT 831215,'Modify Sales',50,19,null,null,0
/* Begin Enhancement - JScott - Export Method - Part 2 */
exec SP_MID_SET_TEXT 831216,'Export',50,19,null,null,0
/* End Enhancement - JScott - Export Method - Part 2 */
/* Begin Enhancement - KJohnson - Global Unlock */
exec SP_MID_SET_TEXT 831217,'Global Unlock',50,19,null,null,0
/* End Enhancement - KJohnson - Global Unlock */
/* Begin Enhancement - KJohnson - Multi Level Spread */
exec SP_MID_SET_TEXT 831218,'Multi Level Spread',50,19,null,null,0
/* End Enhancement - KJohnson - Multi Level Spread */
/* Begin Enhancement - KJohnson - Rollup */
exec SP_MID_SET_TEXT 831219,'Rollup',50,19,null,null,0
/* End Enhancement - KJohnson - Rollup */
exec SP_MID_SET_TEXT 831220,'Global Lock',50,19,null,null,0	/* TT#43 – MD – DOConnell – Projected Sales Enhancement */
exec SP_MID_SET_TEXT 831222,'Planning Extract',50,19,null,null,0	/* TT#2131-MD - JSmith - Halo Integration */
exec SP_MID_SET_TEXT 832000,'Views',50,19,null,null,0
exec SP_MID_SET_TEXT 832100,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 832200,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 833000,'Review',50,19,null,null,0
exec SP_MID_SET_TEXT 833100,'Single-level Chain',50,19,null,null,0
exec SP_MID_SET_TEXT 833200,'Multi-level Chain',50,19,null,null,0
exec SP_MID_SET_TEXT 833300,'Single-level Store',50,19,null,null,0
exec SP_MID_SET_TEXT 833400,'Multi-level Store',50,19,null,null,0
/*Begin Track Track #3735 - JScott - Add security for matrix balance */
exec SP_MID_SET_TEXT 833500,'Matrix Balance',50,19,null,null,0
/*End Track Track #3735 - JScott - Add security for matrix balance */
exec SP_MID_SET_TEXT 834000,'Workflows',50,19,null,null,0
exec SP_MID_SET_TEXT 834100,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 834200,'User',50,19,null,null,0

exec SP_MID_SET_TEXT 840000,'Tools',50,19,null,null,0
exec SP_MID_SET_TEXT 841000,'Audit',50,19,null,null,0
exec SP_MID_SET_TEXT 842000,'Store Filters',50,19,null,null,0
/* Begin TT#1313-MD -jsobek -Header Filters */
exec SP_MID_SET_TEXT 842300,'Header Filters',50,19,null,null,0 
exec SP_MID_SET_TEXT 842400,'Assortment Filters',50,19,null,null,0
/* End TT#1313-MD -jsobek -Header Filters */
exec SP_MID_SET_TEXT 842100,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 842200,'User',50,19,null,null,0
/* Begin TT#1313-MD -jsobek -Header Filters */
exec SP_MID_SET_TEXT 842310,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 842320,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 842410,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 842420,'User',50,19,null,null,0
/* End TT#1313-MD -jsobek -Header Filters */
exec SP_MID_SET_TEXT 843000,'Release Resources',50,19,null,null,0
exec SP_MID_SET_TEXT 843100,'Personal',50,19,null,null,0
exec SP_MID_SET_TEXT 843200,'Others',50,19,null,null,0
exec SP_MID_SET_TEXT 844000,'Text Editor',50,19,null,null,0
exec SP_MID_SET_TEXT 845000,'Scheduler',50,19,null,null,0
exec SP_MID_SET_TEXT 845100,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 845101,'Tasklists',50,19,null,null,0
exec SP_MID_SET_TEXT 845200,'System',50,19,null,null,0
exec SP_MID_SET_TEXT 845201,'Tasklists',50,19,null,null,0
exec SP_MID_SET_TEXT 845202,'Jobs',50,19,null,null,0
exec SP_MID_SET_TEXT 845203,'Special Requests',50,19,null,null,0	/* Issue 5117 stodd */
exec SP_MID_SET_TEXT 845300,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 845301,'Tasklists',50,19,null,null,0
exec SP_MID_SET_TEXT 845400,'Browser',50,19,null,null,0
exec SP_MID_SET_TEXT 845401,'Process Control',50,19,null,null,0		/* TT#1581-MD - stodd - API Header Reconcile */
exec SP_MID_SET_TEXT 845500,'Show Login',50,19,null,null,0
exec SP_MID_SET_TEXT 846000,'Audit - Reclass',50,19,null,null,0
/*BEGIN TT#506 -jsobek –Add infrastructure to allow Email to be sent from the application */
exec [dbo].[SP_MID_SET_TEXT] 847000,'Email Message',50,19,null,null,0 
/*END TT#506 -jsobek –Add infrastructure to allow Email to be sent from the application */
/*BEGIN TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis */
exec [dbo].[SP_MID_SET_TEXT] 848000,'Allocation Analysis',50,19,null,null,0 
exec [dbo].[SP_MID_SET_TEXT] 848100,'Forecast Analysis',50,19,null,null,0 
/*END TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis */
/* Begin TT#855-MD -jsobek -Velocity Enhancements  */ 
/* eVelocityMethodGradeVariableType */
exec SP_MID_SET_TEXT 848200,'Sales',30,1,null,null,0
exec SP_MID_SET_TEXT 848201,'Stock',30,1,null,null,0
/* End TT#855-MD -jsobek -Velocity Enhancements  */ 

/* New for Assortment */
exec SP_MID_SET_TEXT 850000,'Assortment',50,19,null,null,0
exec SP_MID_SET_TEXT 851000,'Methods',50,19,null,null,0
exec SP_MID_SET_TEXT 851100,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 851110,'General Assortment',50,19,null,null,0
exec SP_MID_SET_TEXT 851200,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 851210,'General Assortment',50,19,null,null,0
exec SP_MID_SET_TEXT 852000,'Views',50,19,null,null,0
exec SP_MID_SET_TEXT 852100,'Global',50,19,null,null,0
--exec SP_MID_SET_TEXT 852110,'Assortment Review',50,19,null,null,0		/* TT#1409-md - stodd - assortment view security wrong */  /* TT#1995-MD - JSmith - Security - Assortment Views */
exec SP_MID_SET_TEXT 852200,'User',50,19,null,null,0
--exec SP_MID_SET_TEXT 852210,'Assortment Review',50,19,null,null,0		/* TT#1409-md - stodd - assortment view security wrong */  /* TT#1995-MD - JSmith - Security - Assortment Views */
exec SP_MID_SET_TEXT 853000,'Review',50,19,null,null,0
exec SP_MID_SET_TEXT 853100,'Assortment',50,19,null,null,0
exec SP_MID_SET_TEXT 853200,'Content',50,19,null,null,0
exec SP_MID_SET_TEXT 853300,'Characteristic',50,19,null,null,0
exec SP_MID_SET_TEXT 854000,'Workflows',50,19,null,null,0
exec SP_MID_SET_TEXT 854100,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 854200,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 855000,'Properties',50,19,null,null,0
exec SP_MID_SET_TEXT 856000,'Actions',50,19,null,null,0
exec SP_MID_SET_TEXT 856001,'Redo',50,19,null,null,0
exec SP_MID_SET_TEXT 856002,'Cancel Assortment',50,19,null,null,0
exec SP_MID_SET_TEXT 856003,'Spread Average',50,19,null,null,0
exec SP_MID_SET_TEXT 856004,'Create Placeholders',50,19,null,null,0
exec SP_MID_SET_TEXT 856005,'Balance Assortment',50,19,null,null,0
exec SP_MID_SET_TEXT 856006,'Charge Committed',50,19,null,null,0
exec SP_MID_SET_TEXT 856007,'Cancel Committed',50,19,null,null,0
exec SP_MID_SET_TEXT 856008,'Charge Intransit',50,19,null,null,0
exec SP_MID_SET_TEXT 856009,'Cancel Intransit',50,19,null,null,0
/* End New for Assortment */

/* Explorers */
exec SP_MID_SET_TEXT 860000,'Explorers',50,19,null,null,0
exec SP_MID_SET_TEXT 861000,'Merchandise',50,19,null,null,0
exec SP_MID_SET_TEXT 861100,'Folders',50,19,null,null,0
exec SP_MID_SET_TEXT 861110,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 861120,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 862000,'Store',50,19,null,null,0
exec SP_MID_SET_TEXT 862100,'Folders',50,19,null,null,0
exec SP_MID_SET_TEXT 862110,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 862120,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 863000,'Task List',50,19,null,null,0
exec SP_MID_SET_TEXT 863100,'Folders',50,19,null,null,0
exec SP_MID_SET_TEXT 863110,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 863120,'User',50,19,null,null,0
/* Begin Track #6321 - JScott - User has ability to to create folders when security is view */
exec SP_MID_SET_TEXT 863130,'System',50,19,null,null,0
/* End Track #6321 - JScott - User has ability to to create folders when security is view */
exec SP_MID_SET_TEXT 864000,'Store Filter',50,19,null,null,0
exec SP_MID_SET_TEXT 864100,'Folders',50,19,null,null,0
exec SP_MID_SET_TEXT 864110,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 864120,'User',50,19,null,null,0
/* Begin TT#1313-MD -jsobek -Header Filters */
exec SP_MID_SET_TEXT 864200,'Header Filter',50,19,null,null,0
exec SP_MID_SET_TEXT 864210,'Folders',50,19,null,null,0
exec SP_MID_SET_TEXT 864220,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 864230,'User',50,19,null,null,0

exec SP_MID_SET_TEXT 864300,'Assortment Filter',50,19,null,null,0
exec SP_MID_SET_TEXT 864310,'Folders',50,19,null,null,0
exec SP_MID_SET_TEXT 864320,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 864330,'User',50,19,null,null,0
/* End TT#1313-MD -jsobek -Header Filters */
exec SP_MID_SET_TEXT 865000,'Workflow/Method',50,19,null,null,0
exec SP_MID_SET_TEXT 865100,'Folders',50,19,null,null,0
exec SP_MID_SET_TEXT 865110,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 865120,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 866000,'Allocation Workspace',50,19,null,null,0
exec SP_MID_SET_TEXT 866100,'Views',50,19,null,null,0
exec SP_MID_SET_TEXT 866110,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 866120,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 867000,'Assortment',50,19,null,null,0
/* Begin TT#2 - JSmith - Assortment Security */
--exec SP_MID_SET_TEXT 867100,'Folders',50,19,null,null,0  
exec SP_MID_SET_TEXT 867100,'Explorer',50,19,null,null,0
exec SP_MID_SET_TEXT 867110,'Folders',50,19,null,null,0
exec SP_MID_SET_TEXT 867111,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 867112,'User',50,19,null,null,0
exec SP_MID_SET_TEXT 867200,'Workspace',50,19,null,null,0
exec SP_MID_SET_TEXT 867210,'Views',50,19,null,null,0
exec SP_MID_SET_TEXT 867201,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 867202,'User',50,19,null,null,0
/* End TT#2 */

exec SP_MID_SET_TEXT 870000,'Reports',50,19,null,null,0
exec SP_MID_SET_TEXT 870100,'Audit - Reclass',50,19,null,null,0
exec SP_MID_SET_TEXT 870200,'Node Properties Overrides',50,19,null,null,0
exec SP_MID_SET_TEXT 870300,'Forecast Audit Merchandise',50,19,null,null,0
exec SP_MID_SET_TEXT 870500,'Allocation Audit',50,19,null,null,0
exec SP_MID_SET_TEXT 870600,'Custom',50,19,null,null,0

/* TT#1007 - md - stodd - change group allocation security -  */
exec SP_MID_SET_TEXT 880000,'Group Allocation',50,19,null,null,0
exec SP_MID_SET_TEXT 881000,'Review',50,19,null,null,0
exec SP_MID_SET_TEXT 881100,'Matrix',50,19,null,null,0
exec SP_MID_SET_TEXT 881200,'Content',50,19,null,null,0
exec SP_MID_SET_TEXT 881300,'Characteristics',50,19,null,null,0
exec SP_MID_SET_TEXT 882000,'Actions',50,19,null,null,0
/* TT#1007 - md - stodd - change group allocation security -  */
/* Begin TT#1077 - MD - stodd - cannot create GA views */
exec SP_MID_SET_TEXT 883000,'Views',50,19,null,null,0
exec SP_MID_SET_TEXT 883100,'Global',50,19,null,null,0
exec SP_MID_SET_TEXT 883200,'User',50,19,null,null,0
/* End TT#1077 - MD - stodd - cannot create GA views */

/* Labels */
exec SP_MID_SET_TEXT 900000,'Store ID',50,9,12,0,0
exec SP_MID_SET_TEXT 900001,'Store Name',50,9,12,0,0
exec SP_MID_SET_TEXT 900002,'Active',50,1,12,1,0
exec SP_MID_SET_TEXT 900003,'City',50,9,12,0,0
exec SP_MID_SET_TEXT 900004,'State',50,9,12,0,0
exec SP_MID_SET_TEXT 900005,'Selling Sq. Ft.',50,9,12,3,0
exec SP_MID_SET_TEXT 900006,'Selling Open Dt.',50,9,12,2,0
exec SP_MID_SET_TEXT 900007,'Selling Close Dt.',50,9,12,2,0
exec SP_MID_SET_TEXT 900008,'Stock Open Dt.',50,9,12,2,0
exec SP_MID_SET_TEXT 900009,'Stock Close Dt.',50,9,12,2,0
exec SP_MID_SET_TEXT 900010,'Lead Time',50,9,12,3,0
exec SP_MID_SET_TEXT 900011,'Ship On Mon',50,9,12,1,0
exec SP_MID_SET_TEXT 900012,'Ship On Tue',50,9,12,1,0
exec SP_MID_SET_TEXT 900013,'Ship On Wed',50,9,12,1,0
exec SP_MID_SET_TEXT 900014,'Ship On Thu',50,9,12,1,0
exec SP_MID_SET_TEXT 900015,'Ship On Fri',50,9,12,1,0
exec SP_MID_SET_TEXT 900016,'Ship On Sat',50,9,12,1,0
exec SP_MID_SET_TEXT 900017,'Ship On Sun',50,9,12,1,0
exec SP_MID_SET_TEXT 900018,'New',50,9,1,1,0
exec SP_MID_SET_TEXT 900019,'Comp',50,9,1,1,0
exec SP_MID_SET_TEXT 900020,'NonComp',50,9,1,1,0
exec SP_MID_SET_TEXT 900021,'Store Attribute Builder',50,9,1,0,0
exec SP_MID_SET_TEXT 900022,'Store Status',50,8,13,0,0 
exec SP_MID_SET_TEXT 900023,'Method',50,17,13,0,0
exec SP_MID_SET_TEXT 900024,'Store Description',50,9,12,0,0
exec SP_MID_SET_TEXT 900025,'Hierarchy Properties',50,1,2,0,0		/* Hierarchy Properties form */    	
exec SP_MID_SET_TEXT 900026,'Name:',50,1,2,0,0					/* Hierarchy Name */  				
exec SP_MID_SET_TEXT 900027,'Type',50,1,2,0,0						/* Hierarchy Type */  				
exec SP_MID_SET_TEXT 900028,'Organizational',50,1,2,0,0			/* Hierarchy Type Organizational */  				
exec SP_MID_SET_TEXT 900029,'Alternate',50,1,2,0,0				/* Hierarchy Type Alternate */  				
exec SP_MID_SET_TEXT 900030,'Level Information',50,1,2,0,0		/* Hierarchy Level Information group box */  				
exec SP_MID_SET_TEXT 900031,'Levels',50,1,2,0,0					/* Hierarchy Levels */  				
exec SP_MID_SET_TEXT 900032,'Name:',50,1,2,0,0					/* Hierarchy Level Name */  				
exec SP_MID_SET_TEXT 900033,'Level Properties',50,1,2,0,0			/* Hierarchy Level Properties group box */  				
exec SP_MID_SET_TEXT 900034,'OTS Information',50,1,2,0,0			/* Hierarchy Level OTS Information group box */  				
/*Begin Track #3863 - JScott - OTS Forecast Level Defaults */
/*exec SP_MID_SET_TEXT 900035,'Type',50,1,2,0,0	*/					/* Hierarchy Level OTS Information Type */  				
exec SP_MID_SET_TEXT 900035,'OTS Type',50,1,2,0,0					/* Hierarchy OTS Type */  				
/*End Track #3863 - JScott - OTS Forecast Level Defaults */
exec SP_MID_SET_TEXT 900036,'Regular',50,1,2,0,0					/* Hierarchy Level OTS Regular */  				
exec SP_MID_SET_TEXT 900037,'Total',50,1,2,0,0					/* Hierarchy Level OTS Total */  				
exec SP_MID_SET_TEXT 900038,'Length',50,1,2,0,0					/* Hierarchy Level Length group box */  				
exec SP_MID_SET_TEXT 900039,'Unrestricted',50,1,2,0,0				/* Hierarchy Level Length Unrestricted */  				
exec SP_MID_SET_TEXT 900040,'Required Size',50,1,2,0,0			/* Hierarchy Level Length Required Size */  				
exec SP_MID_SET_TEXT 900041,'Range from',50,1,2,0,0				/* Hierarchy Level Length Range from */  				
exec SP_MID_SET_TEXT 900042,'to',50,1,2,0,0						/* Hierarchy Level Length Range to */  				
exec SP_MID_SET_TEXT 900043,'New',50,1,2,0,0						/* Button - New */  				
exec SP_MID_SET_TEXT 900044,'Delete',50,1,2,0,0					/* Button - Delete */  				
exec SP_MID_SET_TEXT 900045,'Update',50,1,2,0,0					/* Button - Update */  				
exec SP_MID_SET_TEXT 900046,'OK',50,1,2,0,0						/* Button - OK */  				
exec SP_MID_SET_TEXT 900047,'Cancel',50,1,2,0,0					/* Button - Cancel */  
exec SP_MID_SET_TEXT 900048,'Name:',50,1,2,0,0						/* Node Name */  
exec SP_MID_SET_TEXT 900049,'Profile',50,1,2,0,0					/* Tab - Profile */  
exec SP_MID_SET_TEXT 900050,'Basic Replenishment',50,1,2,0,0		/* Basic Replenishment group box */  
exec SP_MID_SET_TEXT 900051,'No',50,1,2,0,0						/* No */  
exec SP_MID_SET_TEXT 900052,'Yes',50,1,2,0,0						/* Yes */  
exec SP_MID_SET_TEXT 900053,'Daily Percentages:',50,1,2,0,0		/* Daily Percentages */  
/*Begin Track #3863 - JScott - OTS Forecast Level Defaults */
/* exec SP_MID_SET_TEXT 900054,'OTS Forecast Level Override',50,1,2,0,0 */	/* OTS Plan Level Override group box */  
exec SP_MID_SET_TEXT 900054,'OTS Forecast',50,1,2,0,0			/* OTS Plan Level Override group box */  
/*End Track #3863 - JScott - OTS Forecast Level Defaults */
exec SP_MID_SET_TEXT 900055,'Override OTS Forecast Level',50,1,2,0,0	/* OTS Override */  
exec SP_MID_SET_TEXT 900056,'Product Type',50,1,2,0,0				/* Product Type group box */  
exec SP_MID_SET_TEXT 900057,'Softlines',50,1,2,0,0					/* Softline */  
exec SP_MID_SET_TEXT 900058,'Hardlines',50,1,2,0,0					/* Hardline */  
exec SP_MID_SET_TEXT 900059,'Undefined',50,1,2,0,0				/* Undefined */  
exec SP_MID_SET_TEXT 900060,'ID:',50,1,2,0,0						/* Node ID */  
exec SP_MID_SET_TEXT 900061,'Security',50,1,2,0,0					/* Button - Security */  
exec SP_MID_SET_TEXT 900062,'Description:',50,1,2,0,0				/* Node Description */  
exec SP_MID_SET_TEXT 900063,'Store Eligibility',50,1,2,0,0		/* Store Eligibility */  
exec SP_MID_SET_TEXT 900064,'Advanced',50,1,2,0,0					/* Button - Advanced */  
exec SP_MID_SET_TEXT 900065,'Store Set',50,1,2,0,0				/* Store Set */  
exec SP_MID_SET_TEXT 900066,'Store Attribute',50,1,2,0,0			/* Store Attribute */  
exec SP_MID_SET_TEXT 900067,'Store Grades',50,1,2,0,0				/* Store Grades */  
exec SP_MID_SET_TEXT 900068,'Store Capacity',50,1,2,0,0			/* Store Capacity */  
exec SP_MID_SET_TEXT 900069,'Velocity Grades',50,1,2,0,0			/* Velocity Grades */  
exec SP_MID_SET_TEXT 900070,'Purge Criteria',50,1,2,0,0			/* Purge Criteria */  
exec SP_MID_SET_TEXT 900071,'Purge Headers after',50,1,2,0,0		/* Purge Headers after */  
exec SP_MID_SET_TEXT 900072,'Weeks',50,1,2,0,0					/* Weeks */  
exec SP_MID_SET_TEXT 900073,'Days',50,1,2,0,0						/* Days */  
exec SP_MID_SET_TEXT 900074,'Purge OTS Forecasts after',50,1,2,0,0	/* Purge weeklyh plans after */  
exec SP_MID_SET_TEXT 900075,'Purge daily history after',50,1,2,0,0/* Purge history after */   
exec SP_MID_SET_TEXT 900076,'No Color',50,0,0,0,0
/* exec SP_MID_SET_TEXT 900076,'Forecast Rule Set:',50,1,2,0,0 */	/* Forecast Rule Set */   
/* exec SP_MID_SET_TEXT 900077,'Allocation Rule Set:',50,1,2,0,0 */	/* Allocation Rule Set */  
/* exec SP_MID_SET_TEXT 900078,'Allocation Rules:',50,1,2,0,0 */		/* Allocation Rules */  
/* exec SP_MID_SET_TEXT 900079,'Forecast Rules:',50,1,2,0,0 */		/* Forecast Rules */  
/* exec SP_MID_SET_TEXT 900080,'Post-Allocation Rules:',50,1,2,0,0 */ 	/* Post-Allocation Rules */  
/* exec SP_MID_SET_TEXT 900081,'Pre-Allocation Rules:',50,1,2,0,0 */	/* Pre-Allocation Rules */  
exec SP_MID_SET_TEXT 900082,'?',50,1,2,0,0						/* Button - Help */  
exec SP_MID_SET_TEXT 900083,'Sort Ascending',50,1,2,0,0			/* Menu - Sort Ascending */  
exec SP_MID_SET_TEXT 900084,'Sort Descending',50,1,2,0,0			/* Menu - Sort Descending */  
exec SP_MID_SET_TEXT 900085,'Search',50,1,2,0,0					/* Menu - Search */  
exec SP_MID_SET_TEXT 900086,'Eligibility',50,1,2,0,0				/* Eligibility */  
exec SP_MID_SET_TEXT 900087,'Ineligible',50,1,2,0,0				/* Ineligible */  
exec SP_MID_SET_TEXT 900088,'Stock Modifier',50,1,2,0,0			/* Stock Modifier */  
exec SP_MID_SET_TEXT 900089,'Sales Modifier',50,1,2,0,0			/* Sales Modifier */  
exec SP_MID_SET_TEXT 900090,'Similar Store',50,1,2,0,0			/* Similar Store */  
exec SP_MID_SET_TEXT 900091,'Index',50,1,2,0,0					/* Ratio */  
exec SP_MID_SET_TEXT 900092,'Time Period',50,1,2,0,0					/* Until */  
exec SP_MID_SET_TEXT 900093,'Grade',50,1,2,0,0					/* Grade */  
exec SP_MID_SET_TEXT 900094,'Boundary',50,1,2,0,0					/* Boundary */  
exec SP_MID_SET_TEXT 900095,'WOS Index',50,1,2,0,0				/* WOS Index */  
/* exec SP_MID_SET_TEXT 900096,'Allocation Min',50,1,2,0,0 */			/* Min Stock */  
/* exec SP_MID_SET_TEXT 900097,'Allocation Max',50,1,2,0,0 */			/* Max Stock */  
exec SP_MID_SET_TEXT 900096,'Minimum',50,1,2,0,0 			/* Min Stock */  /* TT#3510 - JSmith - Velocity Method-> Grade Tab-> Minimum is not spelled correctly */
exec SP_MID_SET_TEXT 900097,'Maximum',50,1,2,0,0 			/* Max Stock */  
exec SP_MID_SET_TEXT 900098,'Min Ad',50,1,2,0,0					/* Min Ad */  
exec SP_MID_SET_TEXT 900099,'Max Ad',50,1,2,0,0					/* Max Ad */  
exec SP_MID_SET_TEXT 900100,'Color Min',50,1,2,0,0				/* Color Min */  
exec SP_MID_SET_TEXT 900101,'Color Max',50,1,2,0,0				/* Color Max */  
exec SP_MID_SET_TEXT 900102,'Sell Thru %',50,1,2,0,0				/* Sell Thru % */  
exec SP_MID_SET_TEXT 900103,'Max Capacity',50,1,2,0,0				/* Max Capacity */  
exec SP_MID_SET_TEXT 900104,'Cut',50,1,2,0,0						/* Menu - Cut */  
exec SP_MID_SET_TEXT 900105,'Copy',50,1,2,0,0						/* Menu - Copy */  
exec SP_MID_SET_TEXT 900106,'Paste',50,1,2,0,0					/* Menu - Paste */  
exec SP_MID_SET_TEXT 900107,'Delete',50,1,2,0,0					/* Menu - Delete */  
exec SP_MID_SET_TEXT 900108,'Insert',50,1,2,0,0					/* Menu - Insert */  
exec SP_MID_SET_TEXT 900109,'Before',50,1,2,0,0					/* Menu - Insert Before */  
exec SP_MID_SET_TEXT 900110,'After',50,1,2,0,0					/* Menu - Insert After */  
exec SP_MID_SET_TEXT 900111,'Rules',50,1,2,0,0					/* Tab - Rules */  
exec SP_MID_SET_TEXT 900112,'Display:',50,1,2,0,0					/* Display Option */  
exec SP_MID_SET_TEXT 900113,'ID Format:',50,1,2,0,0				/* ID Option */  
exec SP_MID_SET_TEXT 900114,'Node Properties',50,1,2,0,0			/* Node Properties */  		
exec SP_MID_SET_TEXT 900115,'New Node',50,1,2,0,0					/* New Node */  		
exec SP_MID_SET_TEXT 900116,'Color Code',50,1,2,0,0				/* Color Code */  	
exec SP_MID_SET_TEXT 900117,'New Hierarchy',50,1,2,0,0			/* New Hierarchy */  	
exec SP_MID_SET_TEXT 900118,'New Personal Hierarchy',50,1,2,0,0	/* New Personal Hierarchy */  	
exec SP_MID_SET_TEXT 900119,'New Level',50,1,2,0,0				/* New Level */  	
exec SP_MID_SET_TEXT 900120,'Levels',50,1,2,0,0					/* Levels */  	
exec SP_MID_SET_TEXT 900121,'Hierarchy Maintenance',50,1,2,0,0	/* Hierarchy Maintenance */  	
exec SP_MID_SET_TEXT 900122,'Merchandise Explorer',50,1,2,0,0		/* Merchandise Explorer */  	
exec SP_MID_SET_TEXT 900123,'Unassigned',50,1,2,0,0				/* Unassigned */  	

/*"English" SQL statement translation*/
exec SP_MID_SET_TEXT 900124,'False',50,1,2,0,0
exec SP_MID_SET_TEXT 900125,'True',50,1,2,0,0
exec SP_MID_SET_TEXT 900126,'Equals',50,1,2,0,0
exec SP_MID_SET_TEXT 900127,'Greater Than',50,1,2,0,0
exec SP_MID_SET_TEXT 900128,'Less Than',50,1,2,0,0
exec SP_MID_SET_TEXT 900129,'Between',50,1,2,0,0
exec SP_MID_SET_TEXT 900130,'Not Equal',50,1,2,0,0
exec SP_MID_SET_TEXT 900131,'Like',50,1,2,0,0
exec SP_MID_SET_TEXT 900132,'In',50,1,2,0,0
exec SP_MID_SET_TEXT 900133,'Not In',50,1,2,0,0
exec SP_MID_SET_TEXT 900134,'And',50,1,2,0,0	
exec SP_MID_SET_TEXT 900135,'Purge weekly history after',50,1,2,0,0/* Purge weekly history after */  	
exec SP_MID_SET_TEXT 900136,'Save',50,1,2,0,0					/* Button - Save */  
exec SP_MID_SET_TEXT 900137,'Color Group:',50,1,2,0,0			/* Color Group */  
exec SP_MID_SET_TEXT 900138,'Apply',50,1,2,0,0				/* Button - Apply */  
exec SP_MID_SET_TEXT 900139,'Date Range',50,1,2,0,0			/* Date Range */  
exec SP_MID_SET_TEXT 900140,'Day 1',50,1,2,0,0				/* Day 1 */  
exec SP_MID_SET_TEXT 900141,'Day 2',50,1,2,0,0				/* Day 2 */  
exec SP_MID_SET_TEXT 900142,'Day 3',50,1,2,0,0				/* Day 3 */  
exec SP_MID_SET_TEXT 900143,'Day 4',50,1,2,0,0				/* Day 4 */  
exec SP_MID_SET_TEXT 900144,'Day 5',50,1,2,0,0				/* Day 5 */  
exec SP_MID_SET_TEXT 900145,'Day 6',50,1,2,0,0				/* Day 6 */  
exec SP_MID_SET_TEXT 900146,'Day 7',50,1,2,0,0				/* Day 7 */  
exec SP_MID_SET_TEXT 900147,'Total %',50,1,2,0,0				/* Total % */  
exec SP_MID_SET_TEXT 900148,'Apply',50,1,2,0,0				/* Menu - Apply */  
exec SP_MID_SET_TEXT 900149,'Clear Values',50,1,2,0,0			/* Menu - Clear */  
exec SP_MID_SET_TEXT 900150,'Reset Messages',50,1,2,0,0		/* Menu - Reset Messages */  
exec SP_MID_SET_TEXT 900151,'Expand All',50,1,2,0,0			/* Menu - Expand All */  
exec SP_MID_SET_TEXT 900152,'Collapse All',50,1,2,0,0			/* Menu - Collapse All */  
exec SP_MID_SET_TEXT 900153,'Add Date Range',50,1,2,0,0		/* Menu/Button - Add Date Range */  
exec SP_MID_SET_TEXT 900154,'Delete Date Range',50,1,2,0,0	/* Menu/Button - Delete Date Range */  
exec SP_MID_SET_TEXT 900155,'Apply all columns',50,1,2,0,0	/* Menu - Apply to all */  
exec SP_MID_SET_TEXT 900156,'Apply selected column',50,1,2,0,0	/* Menu - Apply to column */  
exec SP_MID_SET_TEXT 900157,'Clear all columns',50,1,2,0,0	/* Menu - Apply to all */  
exec SP_MID_SET_TEXT 900158,'Clear selected column',50,1,2,0,0	/* Menu - Apply to column */  
exec SP_MID_SET_TEXT 900159,'Make equal 100',50,1,2,0,0		/* Menu - Balance to 100 */  
exec SP_MID_SET_TEXT 900160,'Open',50,1,2,0,0	/* Store Open */  
exec SP_MID_SET_TEXT 900161,'Closed',50,1,2,0,0	/* Store Closed */  
exec SP_MID_SET_TEXT 900162,'Apply to lower levels',50,1,2,0,0	/* Apply to lower levels */  
exec SP_MID_SET_TEXT 900163,'Selected',50,1,2,0,0				/* Selected */  
exec SP_MID_SET_TEXT 900164,'(Average of stores)',50,1,2,0,0		/* Average similar stores */  
exec SP_MID_SET_TEXT 900165,'Delete All',50,1,2,0,0		/* Average similar stores */  

exec SP_MID_SET_TEXT 900166,'Header',50,1,2,0,0		/* Allocation Header Selection Screen stores */  
exec SP_MID_SET_TEXT 900167,'Description',50,1,2,0,0	/* Allocation Header Selection Screen stores */  
/* exec SP_MID_SET_TEXT 900168,'General Allocation Method',50,17,13,0,0 */
exec SP_MID_SET_TEXT 900169,'Inherited from ',50,17,13,0,0	/* Inherited from */  

exec SP_MID_SET_TEXT 900170,'Preopen',50,9,1,1,0
exec SP_MID_SET_TEXT 900171,'Process',50,1,2,0,0				/* Button - Process */  				
exec SP_MID_SET_TEXT 900172,'Posting Date:',50,1,2,0,0		/* label Posting Date */  	
exec SP_MID_SET_TEXT 900173,'Date not set',50,1,2,0,0			/* label date not set */  
exec SP_MID_SET_TEXT 900174,'Purge headers after',50,1,2,0,0	/* Purge headers after */  
exec SP_MID_SET_TEXT 900175,'OTS Forecast Level',50,1,2,0,0	/* OTS Plan Level  */  
exec SP_MID_SET_TEXT 900176,'Close',50,1,2,0,0					/* Button - Close */
exec SP_MID_SET_TEXT 900177,'View',50,1,2,0,0					/* Button - Preview */
exec SP_MID_SET_TEXT 900178,'OTS Forecast',50,1,2,0,0			/* OTS Plan */
exec SP_MID_SET_TEXT 900179,'Get Header',50,1,2,0,0			/* Button - Get Header */
exec SP_MID_SET_TEXT 900180,'Name',50,1,2,0,0			
exec SP_MID_SET_TEXT 900181,'Filter',50,1,2,0,0			
exec SP_MID_SET_TEXT 900182,'Override',50,1,2,0,0			
exec SP_MID_SET_TEXT 900183,'Method/Action',50,1,2,0,0			
exec SP_MID_SET_TEXT 900184,'Method Name',50,1,2,0,0			
exec SP_MID_SET_TEXT 900185,'Component',50,1,2,0,0			
exec SP_MID_SET_TEXT 900186,'Specific',50,1,2,0,0			
exec SP_MID_SET_TEXT 900187,'Review',50,1,2,0,0			
exec SP_MID_SET_TEXT 900188,'Tolerance %',50,1,2,0,0	
exec SP_MID_SET_TEXT 900189,'Themes...',50,1,2,0,0	
exec SP_MID_SET_TEXT 900190,'Attribute',50,1,2,0,0	
exec SP_MID_SET_TEXT 900191,'Attribute Set',50,1,2,0,0	
exec SP_MID_SET_TEXT 900192,'Intransit',50,1,2,0,0	
exec SP_MID_SET_TEXT 900193,'Stores',50,1,2,0,0	
exec SP_MID_SET_TEXT 900194,'All Stores',50,1,2,0,0	
exec SP_MID_SET_TEXT 900195,'Avg WOS',50,1,2,0,0	
exec SP_MID_SET_TEXT 900196,'Rule',50,1,2,0,0	
exec SP_MID_SET_TEXT 900197,'Qty',50,1,2,0,0	
exec SP_MID_SET_TEXT 900198,'Set',50,1,2,0,0	
exec SP_MID_SET_TEXT 900199,'Merchandise',50,1,2,0,0	
exec SP_MID_SET_TEXT 900200,'Beginning',50,1,2,0,0	
exec SP_MID_SET_TEXT 900201,'Shipping To',50,1,2,0,0	
exec SP_MID_SET_TEXT 900202,'Process Interactive',50,1,2,0,0	
exec SP_MID_SET_TEXT 900203,'Velocity Store Detail',50,1,2,0,0	
exec SP_MID_SET_TEXT 900204,'Similar Stores',50,1,2,0,0	
exec SP_MID_SET_TEXT 900205,'Average',50,1,2,0,0	
exec SP_MID_SET_TEXT 900206,'Ship',50,1,2,0,0	
exec SP_MID_SET_TEXT 900207,'Basis',50,1,2,0,0	
exec SP_MID_SET_TEXT 900208,'Header',50,1,2,0,0	
exec SP_MID_SET_TEXT 900209,'Grades',50,1,2,0,0	
exec SP_MID_SET_TEXT 900210,'Matrix',50,1,2,0,0	
exec SP_MID_SET_TEXT 900211,'Stores with No On Hand',50,1,2,0,0
exec SP_MID_SET_TEXT 900212,'Total',50,1,2,0,0
exec SP_MID_SET_TEXT 900213,'Balance',50,1,2,0,0
exec SP_MID_SET_TEXT 900214,'Total Matrix',50,1,2,0,0
exec SP_MID_SET_TEXT 900215,'Warehouse',50,1,2,0,0
exec SP_MID_SET_TEXT 900216,'Percent',50,1,2,0,0
exec SP_MID_SET_TEXT 900217,'Units',50,1,2,0,0
exec SP_MID_SET_TEXT 900218,'Reserve',50,1,2,0,0
exec SP_MID_SET_TEXT 900219,'Target',50,1,2,0,0
exec SP_MID_SET_TEXT 900220,'Settings',50,1,2,0,0
exec SP_MID_SET_TEXT 900221,'Store Grade Time Period',50,1,2,0,0
exec SP_MID_SET_TEXT 900222,'Percent Need Limt',50,1,2,0,0
exec SP_MID_SET_TEXT 900223,'Exceeds Maximum',50,1,2,0,0
exec SP_MID_SET_TEXT 900224,'On-hand',50,1,2,0,0
exec SP_MID_SET_TEXT 900225,'Factor %',50,1,2,0,0
exec SP_MID_SET_TEXT 900226,'Color Multiple',50,1,2,0,0
exec SP_MID_SET_TEXT 900227,'Size Multiple',50,1,2,0,0
exec SP_MID_SET_TEXT 900228,'Exceed Capacity',50,1,2,0,0
exec SP_MID_SET_TEXT 900229,'Constraints',50,1,2,0,0
exec SP_MID_SET_TEXT 900230,'Exceed by %',50,1,2,0,0
exec SP_MID_SET_TEXT 900231,'Color',50,1,2,0,0
exec SP_MID_SET_TEXT 900232,'Minimum',50,1,2,0,0
exec SP_MID_SET_TEXT 900233,'Maximum',50,1,2,0,0
exec SP_MID_SET_TEXT 900234,'Store',50,1,2,0,0
exec SP_MID_SET_TEXT 900235,'Store Grade',50,1,2,0,0
exec SP_MID_SET_TEXT 900236,'Color Min/Max',50,1,2,0,0
exec SP_MID_SET_TEXT 900237,'Pack',50,1,2,0,0
exec SP_MID_SET_TEXT 900238,'Ascending',50,1,2,0,0
exec SP_MID_SET_TEXT 900239,'Descending',50,1,2,0,0
exec SP_MID_SET_TEXT 900240,'Included Stores',50,1,2,0,0
exec SP_MID_SET_TEXT 900241,'Excluded Stores',50,1,2,0,0
exec SP_MID_SET_TEXT 900242,'Quantity',50,1,2,0,0
exec SP_MID_SET_TEXT 900243,'Prior',50,1,2,0,0
exec SP_MID_SET_TEXT 900244,'Group By',50,1,2,0,0
exec SP_MID_SET_TEXT 900245,'View',50,1,2,0,0
exec SP_MID_SET_TEXT 900246,'Need Analysis',50,1,2,0,0
exec SP_MID_SET_TEXT 900247,'Time Period Begin',50,1,2,0,0
exec SP_MID_SET_TEXT 900248,'Time Period End',50,1,2,0,0
exec SP_MID_SET_TEXT 900249,'Allocation Headers',50,1,2,0,0
exec SP_MID_SET_TEXT 900250,'Condition',50,1,2,0,0
exec SP_MID_SET_TEXT 900251,'Value',50,1,2,0,0
exec SP_MID_SET_TEXT 900252,'Variable',50,1,2,0,0

/*CONTEXT MENU'S FOR STORE EXPLORER*/
exec SP_MID_SET_TEXT 900253,'Remove Store From Set',50,1,2,0,0
exec SP_MID_SET_TEXT 900254,'New Attribute',50,1,2,0,0
exec SP_MID_SET_TEXT 900255,'New Attribute Set',50,1,2,0,0
exec SP_MID_SET_TEXT 900256,'New Store',50,1,2,0,0
exec SP_MID_SET_TEXT 900257,'Rename',50,1,2,0,0
exec SP_MID_SET_TEXT 900258,'Store Profile',50,1,2,0,0
exec SP_MID_SET_TEXT 900259,'Refresh Store Explorer',50,1,2,0,0
exec SP_MID_SET_TEXT 900260,'Edit',50,1,2,0,0

exec SP_MID_SET_TEXT 900261,'Total Sales',50,1,2,0,0
exec SP_MID_SET_TEXT 900262,'Avg Sales',50,1,2,0,0
exec SP_MID_SET_TEXT 900263,'Pct Total Sales',50,1,2,0,0
exec SP_MID_SET_TEXT 900264,'Avg Sales Index',50,1,2,0,0
exec SP_MID_SET_TEXT 900265,'Version',50,1,2,0,0
exec SP_MID_SET_TEXT 900266,'Velocity Grade',50,1,2,0,0
exec SP_MID_SET_TEXT 900267,'Total',50,1,2,0,0
exec SP_MID_SET_TEXT 900268,'Headers',50,1,2,0,0
exec SP_MID_SET_TEXT 900269,'Quantity To Allocate',50,1,2,0,0
/* (JEllis) - BEG MID Track #3611: Quick Filter not working in size review */
/* exec SP_MID_SET_TEXT 900270,'Dim/Size',50,1,2,0,0 */
exec SP_MID_SET_TEXT 900270,'Size/Dim',50,1,2,0,0
/* (JEllis) - END MID Track #3611: Quick Filter not working in size review */
exec SP_MID_SET_TEXT 900271,'Dimension',50,1,2,0,0
exec SP_MID_SET_TEXT 900272,'Forecast',50,1,2,0,0
exec SP_MID_SET_TEXT 900273,'Workflow',50,1,2,0,0
-- Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
-- exec SP_MID_SET_TEXT 900274,'Available Stores',50,1,2,0,0
-- End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
exec SP_MID_SET_TEXT 900275,'API Workflow',50,1,2,0,0
exec SP_MID_SET_TEXT 900276,'Allocate',50,1,2,0,0
exec SP_MID_SET_TEXT 900277,'Store',50,1,2,0,0
exec SP_MID_SET_TEXT 900278,'Chain',50,1,2,0,0
exec SP_MID_SET_TEXT 900279,'Multi-Level',50,1,2,0,0
exec SP_MID_SET_TEXT 900280,'Ineligible Stores',50,1,2,0,0
exec SP_MID_SET_TEXT 900281,'Similar Stores',50,1,2,0,0
exec SP_MID_SET_TEXT 900282,'High Level',50,1,2,0,0
exec SP_MID_SET_TEXT 900283,'Low Levels',50,1,2,0,0
exec SP_MID_SET_TEXT 900284,'Time Period',50,1,2,0,0
exec SP_MID_SET_TEXT 900285,'Override Low Level',50,1,2,0,0
exec SP_MID_SET_TEXT 900286,'Add New Basis',50,1,2,0,0
exec SP_MID_SET_TEXT 900287,'Add New Basis Details',50,1,2,0,0
exec SP_MID_SET_TEXT 900288,'Weight',50,1,2,0,0
exec SP_MID_SET_TEXT 900289,'Forecast Type',50,1,2,0,0
exec SP_MID_SET_TEXT 900290,'Display',50,1,2,0,0
exec SP_MID_SET_TEXT 900291,'Exclude',50,1,2,0,0
exec SP_MID_SET_TEXT 900292,'Size Curve',50,1,2,0,0
exec SP_MID_SET_TEXT 900293,'Size Constraints Model',50,1,2,0,0
exec SP_MID_SET_TEXT 900294,'Size Alternates Model',50,1,2,0,0
exec SP_MID_SET_TEXT 900295,'Computation Mode',50,1,2,0,0
exec SP_MID_SET_TEXT 900296,'Iterations',50,1,2,0,0
exec SP_MID_SET_TEXT 900297,'Balance Mode',50,1,2,0,0
exec SP_MID_SET_TEXT 900298,'Options',50,1,2,0,0
exec SP_MID_SET_TEXT 900299,'Properties',50,1,2,0,0
exec SP_MID_SET_TEXT 900300,'Using',50,1,2,0,0
exec SP_MID_SET_TEXT 900301,'Balance',50,1,2,0,0
exec SP_MID_SET_TEXT 900302,'Process',50,1,2,0,0
exec SP_MID_SET_TEXT 900303,'Select the types of headers that can be released',50,1,2,0,0
exec SP_MID_SET_TEXT 900304,'Size Code',50,1,2,0,0
exec SP_MID_SET_TEXT 900305,'(New)',50,1,2,0,0
exec SP_MID_SET_TEXT 900306,'Product Category',50,1,2,0,0
exec SP_MID_SET_TEXT 900307,'Primary',50,1,2,0,0
exec SP_MID_SET_TEXT 900308,'Secondary',50,1,2,0,0
exec SP_MID_SET_TEXT 900309,'Configuration',50,1,2,0,0
exec SP_MID_SET_TEXT 900310,'Assemblies',50,1,2,0,0
exec SP_MID_SET_TEXT 900311,'Add-Ons',50,1,2,0,0
exec SP_MID_SET_TEXT 900312,'<< Hide',50,1,2,0,0
exec SP_MID_SET_TEXT 900313,'Details >>',50,1,2,0,0
exec SP_MID_SET_TEXT 900314,'Allocation',50,1,2,0,0
exec SP_MID_SET_TEXT 900315,'Size',50,1,2,0,0
exec SP_MID_SET_TEXT 900316,'Planning',50,1,2,0,0
exec SP_MID_SET_TEXT 900317,'Assortment',50,1,2,0,0
/* (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement */
exec SP_MID_SET_TEXT 900318,'Master',50,1,2,0,0
exec SP_MID_SET_TEXT 900319,'MasterID',50,1,2,0,0 
exec SP_MID_SET_TEXT 900320,'Allocated Units',50,1,2,0,0 
exec SP_MID_SET_TEXT 900321,'Original Allocated Units',50,1,2,0,0 
exec SP_MID_SET_TEXT 900322,'Reserve Units',50,1,2,0,0 
exec SP_MID_SET_TEXT 900323,'Subordinate',50,1,2,0,0 
exec SP_MID_SET_TEXT 900324,'Reserve Allocated Units',50,1,2,0,0 
/* (CSMITH) - END MID Track #3219                                  */
exec SP_MID_SET_TEXT 900325,'Copy To',50,1,2,0,0
exec SP_MID_SET_TEXT 900326,'MultiHeaderID',50,1,2,0,0
exec SP_MID_SET_TEXT 900327,'HeaderID',50,1,2,0,0
exec SP_MID_SET_TEXT 900328,'Type',50,1,2,0,0
exec SP_MID_SET_TEXT 900329,'Date',50,1,2,0,0
exec SP_MID_SET_TEXT 900330,'UnitRetail',50,1,2,0,0
exec SP_MID_SET_TEXT 900331,'UnitCost',50,1,2,0,0
exec SP_MID_SET_TEXT 900332,'SizeGroup',50,1,2,0,0
exec SP_MID_SET_TEXT 900333,'Multiple',50,1,2,0,0
exec SP_MID_SET_TEXT 900334,'PO',50,1,2,0,0
exec SP_MID_SET_TEXT 900335,'Vendor',50,1,2,0,0
exec SP_MID_SET_TEXT 900336,'DC',50,1,2,0,0
exec SP_MID_SET_TEXT 900337,'Status',50,1,2,0,0
exec SP_MID_SET_TEXT 900338,'ShipStatus',50,1,2,0,0
exec SP_MID_SET_TEXT 900339,'Release',50,1,2,0,0
exec SP_MID_SET_TEXT 900340,'ChildTotal',50,1,2,0,0
exec SP_MID_SET_TEXT 900341,'PackType',50,1,2,0,0
exec SP_MID_SET_TEXT 900342,'TotalPacks',50,1,2,0,0
exec SP_MID_SET_TEXT 900343,'QuantityPerPack',50,1,2,0,0
exec SP_MID_SET_TEXT 900344,'TotalQuantity',50,1,2,0,0
exec SP_MID_SET_TEXT 900345,'PackColor',50,1,2,0,0
exec SP_MID_SET_TEXT 900346,'Description',50,1,2,0,0
exec SP_MID_SET_TEXT 900347,'BulkColor',50,1,2,0,0
exec SP_MID_SET_TEXT 900348,'Description',50,1,2,0,0
exec SP_MID_SET_TEXT 900349,'Restore Layout',50,1,2,0,0
exec SP_MID_SET_TEXT 900350,'Spread From',50,1,2,0,0
exec SP_MID_SET_TEXT 900351,'Description',50,1,2,0,0
exec SP_MID_SET_TEXT 900352,'Similar Store Model',50,9,12,1,0
/*Begin Track #3863 - JScott - OTS Forecast Level Defaults */
exec SP_MID_SET_TEXT 900353,'Level Type',50,1,12,1,0					/* Hierarchy OTS Level Type */ 
exec SP_MID_SET_TEXT 900354,'Level',50,1,12,1,0							/* Hierarchy OTS Node Level */ 
exec SP_MID_SET_TEXT 900355,'Type',50,1,12,1,0							/* Hierarchy OTS Node Type */ 
/*End Track #3863 - JScott - OTS Forecast Level Defaults */
exec SP_MID_SET_TEXT 900356,'None',30,1,2,0,0                          /* MID Track 3914 - J.Ellis - Label for no secondary size */
/*Begin Reclass - JSmith  */
exec SP_MID_SET_TEXT 900357,'Receipts',30,1,2,0,0                          
exec SP_MID_SET_TEXT 900358,'Sales',30,1,2,0,0                          
exec SP_MID_SET_TEXT 900359,'Stock',30,1,2,0,0                          
exec SP_MID_SET_TEXT 900360,'Current On Hand',30,1,2,0,0 
exec SP_MID_SET_TEXT 900361,'Index',30,1,2,0,0                         
exec SP_MID_SET_TEXT 900362,'Add',30,1,2,0,0  
exec SP_MID_SET_TEXT 900363,'Product',30,1,2,0,0
exec SP_MID_SET_TEXT 900364,'Transaction',30,1,2,0,0
exec SP_MID_SET_TEXT 900365,'Move',50,1,2,0,0
exec SP_MID_SET_TEXT 900366,'Delete',50,1,2,0,0
exec SP_MID_SET_TEXT 900367,'Rename',50,1,2,0,0
exec SP_MID_SET_TEXT 900368,'Task List',50,1,2,0,0
/*End Reclass */
exec SP_MID_SET_TEXT 900369,'Stop',30,1,2,0,0
exec SP_MID_SET_TEXT 900370,'All or part of the ID:',30,1,2,0,0
exec SP_MID_SET_TEXT 900371,'All or part of the Name:',30,1,2,0,0
exec SP_MID_SET_TEXT 900372,'All or part of the Description:',30,1,2,0,0	                      
exec SP_MID_SET_TEXT 900373,'At level(s):',30,1,2,0,0
exec SP_MID_SET_TEXT 900374,'&Locate',30,1,2,0,0	                      	
exec SP_MID_SET_TEXT 900375,'Search options',30,1,2,0,0	                      	
exec SP_MID_SET_TEXT 900376,'Match case',30,1,2,0,0	                      	
exec SP_MID_SET_TEXT 900377,'Match entire word',30,1,2,0,0	                      	
exec SP_MID_SET_TEXT 900378,'Select All',30,1,2,0,0	                      	
exec SP_MID_SET_TEXT 900379,'Clear All',30,1,2,0,0	                      		                      
exec SP_MID_SET_TEXT 900380,'Show Details',30,1,2,0,0
exec SP_MID_SET_TEXT 900381,'Remove',30,1,2,0,0
exec SP_MID_SET_TEXT 900382,'Multi-Header',30,1,2,0,0
exec SP_MID_SET_TEXT 900383,'Create',30,1,2,0,0
exec SP_MID_SET_TEXT 900384,'Add to',30,1,2,0,0
exec SP_MID_SET_TEXT 900385,'Remove from',30,1,2,0,0
exec SP_MID_SET_TEXT 900386,'PackSize',30,1,2,0,0
exec SP_MID_SET_TEXT 900387,'BulkSize',30,1,2,0,0
exec SP_MID_SET_TEXT 900388,'AssortmentID',30,1,2,0,0
exec SP_MID_SET_TEXT 900389,'Column Chooser',30,1,2,0,0
exec SP_MID_SET_TEXT 900390,'Header Notes for',30,1,2,0,0
exec SP_MID_SET_TEXT 900391,'Delete Row',30,1,2,0,0
exec SP_MID_SET_TEXT 900392,'User',30,1,2,0,0
exec SP_MID_SET_TEXT 900393,'Number of Placeholder Colors',30,1,2,0,0
exec SP_MID_SET_TEXT 900394,'All or part of the Group Name:',30,1,2,0,0
exec SP_MID_SET_TEXT 900395,'New Characteristic',30,1,2,0,0
exec SP_MID_SET_TEXT 900396,'New Value',30,1,2,0,0
exec SP_MID_SET_TEXT 900397,'All or part of the Value:',30,1,2,0,0
exec SP_MID_SET_TEXT 900398,'All or part of the Characteristic:',30,1,2,0,0
exec SP_MID_SET_TEXT 900399,'Characteristics',30,1,2,0,0
exec SP_MID_SET_TEXT 900400,'Characteristic',30,1,2,0,0
exec SP_MID_SET_TEXT 900401,'Assigned',30,1,2,0,0
exec SP_MID_SET_TEXT 900402,'Hierarchy',30,1,2,0,0
exec SP_MID_SET_TEXT 900403,'Use Characteristics',30,1,2,0,0
exec SP_MID_SET_TEXT 900404,'PlaceholderID',30,1,2,0,0
exec SP_MID_SET_TEXT 900405,'Excel',30,1,2,0,0
exec SP_MID_SET_TEXT 900406,'File',30,1,2,0,0
exec SP_MID_SET_TEXT 900407,'All Sets',30,1,2,0,0
exec SP_MID_SET_TEXT 900408,'Current Set',30,1,2,0,0
exec SP_MID_SET_TEXT 900409,'Formatting',30,1,2,0,0
exec SP_MID_SET_TEXT 900410,'Location',30,1,2,0,0
exec SP_MID_SET_TEXT 900411,'Include',30,1,2,0,0
/*End Reclass */
exec SP_MID_SET_TEXT 900412,'Select action...',30,1,2,0,0
exec SP_MID_SET_TEXT 900413,'Add Placeholder Style',30,1,2,0,0
exec SP_MID_SET_TEXT 900414,'PhStyle',30,1,2,0,0

exec SP_MID_SET_TEXT 900415,'Link Headers with Characteristic',50,1,2,0,0
exec SP_MID_SET_TEXT 900416,'<None>',50,1,2,0,0
/* Label for Secondary Size “all dim” allocated column in Size Review */
exec SP_MID_SET_TEXT 900417,'All Dim',50,1,2,0,0
exec SP_MID_SET_TEXT 900418,'Generic Size Curve',250,0,5,null,0
exec SP_MID_SET_TEXT 900419,'< No Header Characteristic specified >',250,0,5,null,0
exec SP_MID_SET_TEXT 900420,'< No Hierarchy Level specified >',250,0,5,null,0
exec SP_MID_SET_TEXT 900421,'Sales Matrix',50,1,2,0,0
exec SP_MID_SET_TEXT 900422,'Placeholder Style',30,1,2,0,0
exec SP_MID_SET_TEXT 900423,'PHC',30,1,2,0,0
exec SP_MID_SET_TEXT 900424,'Placeholder Color',30,1,2,0,0
exec SP_MID_SET_TEXT 900425,'Hierarchy',30,1,2,0,0
exec SP_MID_SET_TEXT 900426,'Node',30,1,2,0,0
exec SP_MID_SET_TEXT 900427,'Starts with',30,1,2,0,0
exec SP_MID_SET_TEXT 900428,'Save View',30,1,2,0,0
exec SP_MID_SET_TEXT 900429,'Views',30,1,2,0,0
exec SP_MID_SET_TEXT 900430,'View Name',30,1,2,0,0
exec SP_MID_SET_TEXT 900431,'Delete View',30,1,2,0,0
exec SP_MID_SET_TEXT 900432,'Save Filter',30,1,2,0,0
exec SP_MID_SET_TEXT 900433,'hierarchy node',30,1,2,0,0
exec SP_MID_SET_TEXT 900434,'AnchorNode',30,1,2,0,0
exec SP_MID_SET_TEXT 900435,'Protect Interfaced Headers',30,1,2,0,0
/* Labels for A&F Generic Size Constraints */
exec SP_MID_SET_TEXT 900436,'Require Size Drop Down Filters',50,1,2,0,0
exec SP_MID_SET_TEXT 900437,'Character Mask',30,1,2,0,0
exec SP_MID_SET_TEXT 900438,'Size Group',30,1,2,0,0
exec SP_MID_SET_TEXT 900439,'Size Alternates',30,1,2,0,0
exec SP_MID_SET_TEXT 900440,'Size Constraints',30,1,2,0,0
exec SP_MID_SET_TEXT 900441,'Generic',30,1,2,0,0
exec SP_MID_SET_TEXT 900442,'Filter Selection',30,1,2,0,0
exec SP_MID_SET_TEXT 900443,'All or part of the name',50,1,2,0,0
exec SP_MID_SET_TEXT 900444,'FWOS Override',30,1,2,0,0
exec SP_MID_SET_TEXT 900445,'On',30,1,2,0,0
exec SP_MID_SET_TEXT 900446,'Off',30,1,2,0,0
exec SP_MID_SET_TEXT 900447,'Forecast Monitor',30,1,2,0,0
exec SP_MID_SET_TEXT 900448,'Modify Sales Monitor',30,1,2,0,0
exec SP_MID_SET_TEXT 900449,'Audit Logging Level',30,1,2,0,0
exec SP_MID_SET_TEXT 900450,'Directory...',30,1,2,0,0
exec SP_MID_SET_TEXT 900451,'Current Sales Week',30,1,2,0,0
exec SP_MID_SET_TEXT 900452,'Daily Sales Thru',30,1,2,0,0
exec SP_MID_SET_TEXT 900453,'Period',30,1,2,0,0
exec SP_MID_SET_TEXT 900454,'Week',30,1,2,0,0
exec SP_MID_SET_TEXT 900455,'Day',30,1,2,0,0
exec SP_MID_SET_TEXT 900456,'Dynamic',30,1,2,0,0
exec SP_MID_SET_TEXT 900457,'Static',30,1,2,0,0
exec SP_MID_SET_TEXT 900458,'Recurring',30,1,2,0,0
exec SP_MID_SET_TEXT 900459,'Dynamic Switch',30,1,2,0,0
exec SP_MID_SET_TEXT 900460,'Current',30,1,2,0,0
exec SP_MID_SET_TEXT 900461,'Plan',30,1,2,0,0
exec SP_MID_SET_TEXT 900462,'Store Open',30,1,2,0,0
exec SP_MID_SET_TEXT 900463,'Low Levels',30,1,2,0,0
exec SP_MID_SET_TEXT 900464,'Exclude Low Levels',30,1,2,0,0
exec SP_MID_SET_TEXT 900465,'High Level',30,1,2,0,0
exec SP_MID_SET_TEXT 900466,'Stock Min/Max',50,1,2,0,0 
exec SP_MID_SET_TEXT 900467,'Add New Stock Min/Max',50,1,2,0,0 
exec SP_MID_SET_TEXT 900468,'Apply Min/Max',50,1,2,0,0 
exec SP_MID_SET_TEXT 900469,'Inherit From',50,1,2,0,0 
exec SP_MID_SET_TEXT 900470,'Low Level Setting',50,1,2,0,0 
exec SP_MID_SET_TEXT 900471,'Hierarchy',50,1,2,0,0 
exec SP_MID_SET_TEXT 900472,'None',50,1,2,0,0 
/* Begin Track #4457 - JSmith - Add forecast versions */
exec SP_MID_SET_TEXT 900473,'Workflows',30,1,2,0,0
exec SP_MID_SET_TEXT 900474,'Description',30,1,2,0,0
exec SP_MID_SET_TEXT 900475,'Protect History',30,1,2,0,0
exec SP_MID_SET_TEXT 900476,'Active',30,1,2,0,0
exec SP_MID_SET_TEXT 900477,'Combine',30,1,2,0,0
exec SP_MID_SET_TEXT 900478,'Actual',30,1,2,0,0
exec SP_MID_SET_TEXT 900479,'Forecast',30,1,2,0,0
exec SP_MID_SET_TEXT 900480,'Period History',30,1,2,0,0
/* End Track #4457 */
/* Begin Track #4547 - JSmith - Add similar stores by forecast versions */
exec SP_MID_SET_TEXT 900481,'Similar Store',30,1,2,0,0
/* End Track #4547  */
/* Begin Track #4573 - STodd - */
exec SP_MID_SET_TEXT 900483,'Low Level Default',50,1,2,0,0   
/* End Track #4573  */
/* Begin Track #4619 - JSmith - Add auto-upgrade */
exec SP_MID_SET_TEXT 900484,'Incompatible Client',50,1,2,0,0
/* End Track #4619 */ 
exec SP_MID_SET_TEXT 900485,'TY',50,1,2,0,0
exec SP_MID_SET_TEXT 900486,'LY',50,1,2,0,0
exec SP_MID_SET_TEXT 900487,'Apply To',50,1,2,0,0
exec SP_MID_SET_TEXT 900488,'',50,1,2,0,0
/* BEGIN Issue 4884 */
exec SP_MID_SET_TEXT 900489,'TY',50,1,2,0,0
exec SP_MID_SET_TEXT 900490,'LY',50,1,2,0,0
exec SP_MID_SET_TEXT 900491,'Apply Trend To',50,1,2,0,0
exec SP_MID_SET_TEXT 900492,'Trend Caps',50,1,2,0,0
exec SP_MID_SET_TEXT 900493,'Criteria',50,1,2,0,0
exec SP_MID_SET_TEXT 900494,'Stock Min/Max',50,1,2,0,0
exec SP_MID_SET_TEXT 900495,'Set Methods',50,1,2,0,0
/* END Issue 4884 */
/* Begin Track #4815 - JSmith - #283-User (Security) Maintenance */
exec SP_MID_SET_TEXT 900496,'Permanently move user',50,1,2,0,0
exec SP_MID_SET_TEXT 900497,'Assign To',50,1,2,0,0
/* End Track #4815 */
exec SP_MID_SET_TEXT 900498,'Store Grades By Basis',50,1,2,0,0	/* Track 6074 stodd */
exec SP_MID_SET_TEXT 900499,'Default Grades By Basis',50,1,2,0,0	/* Track 6074 stodd */
/* Begin Enhancement - JScott - Export Method - Part 2 */
exec SP_MID_SET_TEXT 900500,'Export Criteria',30,10,null,null,50
exec SP_MID_SET_TEXT 900501,'Low Level',30,10,null,null,50
exec SP_MID_SET_TEXT 900502,'Low Levels Only',30,10,null,null,50
exec SP_MID_SET_TEXT 900503,'Chain Data',30,10,null,null,50
exec SP_MID_SET_TEXT 900504,'Store Data',30,10,null,null,50
exec SP_MID_SET_TEXT 900505,'Export File Options',30,10,null,null,50
exec SP_MID_SET_TEXT 900506,'Variables',30,10,null,null,50
exec SP_MID_SET_TEXT 900507,'Format',30,10,null,null,50
exec SP_MID_SET_TEXT 900508,'Use Default Settings',30,10,null,null,50
exec SP_MID_SET_TEXT 900509,'File Format',30,10,null,null,50
exec SP_MID_SET_TEXT 900510,'CSV File',30,10,null,null,50
exec SP_MID_SET_TEXT 900511,'XML File',30,10,null,null,50
exec SP_MID_SET_TEXT 900512,'Delimeter',30,10,null,null,50
exec SP_MID_SET_TEXT 900513,'Output Options',30,10,null,null,50
exec SP_MID_SET_TEXT 900514,'File Path',30,10,null,null,50
exec SP_MID_SET_TEXT 900515,'Browse',30,10,null,null,50
exec SP_MID_SET_TEXT 900516,'File Name will be generated from the Merchandise and Level',30,10,null,null,50
exec SP_MID_SET_TEXT 900517,'Add to File Name:',30,10,null,null,50
exec SP_MID_SET_TEXT 900518,'Date Stamp',30,10,null,null,50
exec SP_MID_SET_TEXT 900519,'Time Stamp',30,10,null,null,50
exec SP_MID_SET_TEXT 900520,'Split Files by:',30,10,null,null,50
exec SP_MID_SET_TEXT 900521,'Merchandise',30,10,null,null,50
exec SP_MID_SET_TEXT 900522,'None',30,10,null,null,50
exec SP_MID_SET_TEXT 900523,'Number of entries',30,10,null,null,50
exec SP_MID_SET_TEXT 900524,'Create Flag File',30,10,null,null,50
exec SP_MID_SET_TEXT 900525,'Suffix',30,10,null,null,50
exec SP_MID_SET_TEXT 900526,'Create End File',30,10,null,null,50
exec SP_MID_SET_TEXT 900527,'Suffix',30,10,null,null,50
exec SP_MID_SET_TEXT 900528,'Concurrent Processes',30,10,null,null,50
/* Begin Track #4942 - JScott - Correct problems in Export Method */
/* exec SP_MID_SET_TEXT 900529,'Value Type',30,10,null,null,50 */
exec SP_MID_SET_TEXT 900529,'Output Format',30,10,null,null,50
/* End Track #4942 - JScott - Correct problems in Export Method */
exec SP_MID_SET_TEXT 900530,'Extract Pre-Init Values',30,10,null,null,50
exec SP_MID_SET_TEXT 900531,'Extract Ineligible Stores',30,10,null,null,50
/* End Enhancement - JScott - Export Method - Part 2 */
/* BEGIN MID Track #4827 - John Smith - Presentation plus sales  */
exec SP_MID_SET_TEXT 900532,'Min + Sales',30,10,null,null,50
/* END MID Track #????  */
/* Begin Track #4826 - JSmith - Normalize Size Curves */
exec SP_MID_SET_TEXT 900533,'Normalize Size Curves',50,1,2,0,0   
exec SP_MID_SET_TEXT 900534,'Override Default',50,1,2,0,0   
/* End Track #4573  */

/* Begin Track #4924 - JSmith - Add inherit from higher level  */
exec SP_MID_SET_TEXT 900535,'Inherit from higher level',50,1,2,0,0	/* Inherit from higher level */    
/* End Track #4924  */

/* Begin Track #4942 - JScott - Correct problems in Export Method */
exec SP_MID_SET_TEXT 900536,'Date Type',30,10,null,null,50
exec SP_MID_SET_TEXT 900537,'Calendar',30,10,null,null,50
exec SP_MID_SET_TEXT 900538,'Fiscal',30,10,null,null,50
exec SP_MID_SET_TEXT 900539,'CSV File Suffix',30,10,null,null,50
/* End Track #4942 - JScott - Correct problems in Export Method */
exec SP_MID_SET_TEXT 900540,'Save As',30,10,null,null,50
exec SP_MID_SET_TEXT 900541,'Model Name',30,10,null,null,50
exec SP_MID_SET_TEXT 900542,'Primary Size Curve',30,10,null,null,50
exec SP_MID_SET_TEXT 900543,'Alternate Size Curve',30,10,null,null,50
exec SP_MID_SET_TEXT 900544,'Clear',30,10,null,null,50
exec SP_MID_SET_TEXT 900545,'Verify',30,10,null,null,50
/* BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification  */
exec SP_MID_SET_TEXT 900546,'Fill To',30,10,null,null,50
/* End Track #4921  */
/* BEGIN MID Track #5170 - JSmith - Model enhancements  */
exec SP_MID_SET_TEXT 900547,'Case Sensitive',30,10,null,null,50
/* End Track #5170  */
/* Begin Enhancement #5176 - JSmith - Audit Filter  */
exec SP_MID_SET_TEXT 900548,'Highest Process Message Level',30,10,null,null,50
exec SP_MID_SET_TEXT 900549,'Run Date',30,10,null,null,50
exec SP_MID_SET_TEXT 900550,'All',30,10,null,null,50
exec SP_MID_SET_TEXT 900551,'and',30,10,null,null,50
exec SP_MID_SET_TEXT 900552,'days',30,10,null,null,50
exec SP_MID_SET_TEXT 900554,'to',30,10,null,null,50
exec SP_MID_SET_TEXT 900555,'from',30,10,null,null,50
exec SP_MID_SET_TEXT 900556,'Specify Dates',30,10,null,null,50
exec SP_MID_SET_TEXT 900557,'Between',30,10,null,null,50
exec SP_MID_SET_TEXT 900558,'Today',30,10,null,null,50
exec SP_MID_SET_TEXT 900559,'Highest Detail Message Level',30,10,null,null,50
exec SP_MID_SET_TEXT 900560,'Duration greater than',30,10,null,null,50
exec SP_MID_SET_TEXT 900561,'minutes',30,10,null,null,50
exec SP_MID_SET_TEXT 900562,'Status',30,10,null,null,50
exec SP_MID_SET_TEXT 900563,'Running',30,10,null,null,50
exec SP_MID_SET_TEXT 900564,'Completed',30,10,null,null,50
exec SP_MID_SET_TEXT 900565,'Show My Tasks Only',30,10,null,null,50
exec SP_MID_SET_TEXT 900566,'Unlock',30,10,null,null,50
exec SP_MID_SET_TEXT 900567,'Multi Level',30,10,null,null,50
exec SP_MID_SET_TEXT 900568,'From Level',30,10,null,null,50
exec SP_MID_SET_TEXT 900569,'To Level',30,10,null,null,50
exec SP_MID_SET_TEXT 900570,'Override',30,10,null,null,50
exec SP_MID_SET_TEXT 900571,'Options',30,10,null,null,50
exec SP_MID_SET_TEXT 900572,'Stores',30,10,null,null,50
exec SP_MID_SET_TEXT 900573,'Chain',30,10,null,null,50
exec SP_MID_SET_TEXT 900574,'Store Options',30,10,null,null,50
exec SP_MID_SET_TEXT 900575,'Attribute',30,10,null,null,50
exec SP_MID_SET_TEXT 900576,'Filter',30,10,null,null,50
exec SP_MID_SET_TEXT 900577,'Attribute Set',30,10,null,null,50
exec SP_MID_SET_TEXT 900578,'Last Processed',30,10,null,null,50
exec SP_MID_SET_TEXT 900579,'Date/Time',30,10,null,null,50
exec SP_MID_SET_TEXT 900580,'By User',30,10,null,null,50
exec SP_MID_SET_TEXT 900581,'Special Request Name:',30,10,null,null,50	/* Issue 5117 */
exec SP_MID_SET_TEXT 900582,'Concurrent Jobs',30,10,null,null,50			/* Issue 5117 */
/* End Enhancement #5176  */
exec SP_MID_SET_TEXT 900583,'PH',10,1,2,0,0
exec SP_MID_SET_TEXT 900584,'Add New Rollup Item',30,10,null,null,50
/* Begin Track #5395 - JScott - Add ability to discard zero values in Export */
exec SP_MID_SET_TEXT 900585,'Exclude Zero Values',30,10,null,null,50
/* End Track #5395 - JScott - Add ability to discard zero values in Export */
exec SP_MID_SET_TEXT 900586,'Custom',30,10,null,null,50
exec SP_MID_SET_TEXT 900587,'Model',30,10,null,null,50
exec SP_MID_SET_TEXT 900588,'Low Level',30,10,null,null,50
exec SP_MID_SET_TEXT 900589,'Rollup',30,10,null,null,50

/* BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement */
exec SP_MID_SET_TEXT 900590,'Add Variable',50,1,2,0,0
exec SP_MID_SET_TEXT 900591,'Apply Modifiers',50,1,2,0,0
exec SP_MID_SET_TEXT 900592,'Forecast\nFormula',50,1,2,0,0
exec SP_MID_SET_TEXT 900593,'Associated Variable',50,1,2,0,0
exec SP_MID_SET_TEXT 900594,'Stock\nMin',50,1,2,0,0
exec SP_MID_SET_TEXT 900595,'Stock\nMax',50,1,2,0,0
exec SP_MID_SET_TEXT 900596,'Sales',50,1,2,0,0
exec SP_MID_SET_TEXT 900597,'Stock',50,1,2,0,0
exec SP_MID_SET_TEXT 900598,'Percent Contribution',50,1,2,0,0
/* END MID Track #5773 */
/* BEGIN MID Track #6187 stodd */
exec SP_MID_SET_TEXT 900600,'Use Plan',50,1,2,0,0
exec SP_MID_SET_TEXT 900601,'Basis Overrides',50,1,2,0,0

/* End MID Track #6187 stodd */
/* BEGIN MID Track #6271 stodd */
exec SP_MID_SET_TEXT 900603,'Allow Chain\nNegatives',50,1,2,0,0
/* END MID Track #6171 stodd */
exec SP_MID_SET_TEXT 900604,'Hide Details',50,1,12,3,0		/* Workspace Usability */
/* Begin TT#2 - Ron Matelic - Assortment Planning */
exec SP_MID_SET_TEXT 900605,'Assortments',50,1,2,0,0
exec SP_MID_SET_TEXT 900606,'Assortment Quantities',50,1,2,0,0
/* End TT#2  */
/* Begin TT#2 - stodd - Assortment Planning */
exec SP_MID_SET_TEXT 900607,'Delivery',50,1,2,0,0
exec SP_MID_SET_TEXT 900608,'Committed',50,1,2,0,0
/* End TT#2  */
exec SP_MID_SET_TEXT 900609,'Copy Pre-Init Values',30,10,null,null,50 /* Track #6347 - Copy Store Forecasting seems to be running extremely long  */
/* BEGIN Workspace Usability Enhancement - Ron Matelic */
exec SP_MID_SET_TEXT 900610,'# Packs',30,10,null,null,50		
exec SP_MID_SET_TEXT 900611,'# Bulk Colors',30,10,null,null,50
exec SP_MID_SET_TEXT 900612,'# Bulk Sizes',30,10,null,null,50
/* END Workspace Usability Enhancement   */

/* Begin TT#153 – add variables to velocity matrix - apicchetti */
exec SP_MID_SET_TEXT 900613,'Total_# Stores',30,10,null,null,50
exec SP_MID_SET_TEXT 900614,'Stock_% Total',30,10,null,null,50
exec SP_MID_SET_TEXT 900615,'Avg Stock',30,10,null,null,50
exec SP_MID_SET_TEXT 900616,'Allocation_% Total',30,10,null,null,50
/* End TT#153 – add variables to velocity matrix - apicchetti */

/* Begin TT#154 – Add variables to Allocation workspace - apicchetti */
exec SP_MID_SET_TEXT 900617,'# of Stores',30,10,null,null,50
/* End TT#154 – Add variables to Allocation workspace - apicchetti */

/* Begin TT#152 - Velocity balance - apicchetti */
exec SP_MID_SET_TEXT 900618,'Balance',30,24,null,null,0
/* End TT#152 - Velocity balance - apicchetti */
/* Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail */
exec SP_MID_SET_TEXT 900619,'Velocity Matrix',30,1,2,0,0
/* End TT#231 */
/* Begin TT#155 - JScott - Size Curve Method */
exec SP_MID_SET_TEXT 900620,'Merchandise Basis',30,10,null,null,50
exec SP_MID_SET_TEXT 900621,'Curve Basis',30,10,null,null,50
exec SP_MID_SET_TEXT 900622,'Equalize Weighting:',30,10,null,null,50
/* End TT#155 - JScott - Size Curve Method */
/* Begin TT#155 - JScott - Add Size Curve info to Node Properties */
exec SP_MID_SET_TEXT 900623,'Size Curves',30,10,null,null,50
exec SP_MID_SET_TEXT 900624,'Tolerance',30,10,null,null,50
exec SP_MID_SET_TEXT 900625,'Size Curve Inherited Criteria',30,10,null,null,50
exec SP_MID_SET_TEXT 900626,'Size Curve Criteria',30,10,null,null,50
exec SP_MID_SET_TEXT 900627,'Size Curve Similar Store',30,10,null,null,50
exec SP_MID_SET_TEXT 900628,'Default',30,10,null,null,50
exec SP_MID_SET_TEXT 900629,'Lost Sales',30,10,null,null,50
exec SP_MID_SET_TEXT 900630,'Include/Exclude',30,10,null,null,50
exec SP_MID_SET_TEXT 900631,'Curve Name',30,10,null,null,50
exec SP_MID_SET_TEXT 900632,'Add',30,10,null,null,50
/* Begin TT#274 - RMatelic - Purge Criteria report issues */
exec SP_MID_SET_TEXT 900633,'Modifiers',30,10,null,null,50
exec SP_MID_SET_TEXT 900634,'Allocation Min/Max',30,10,null,null,50
exec SP_MID_SET_TEXT 900635,'Allocation Audit',30,10,null,null,50
exec SP_MID_SET_TEXT 900636,'**',30,10,null,null,50
exec SP_MID_SET_TEXT 900637,'Capacity',30,10,null,null,50
exec SP_MID_SET_TEXT 900638,'Daily %''s',30,10,null,null,50
exec SP_MID_SET_TEXT 900639,'Purge Criteria',30,10,null,null,50
exec SP_MID_SET_TEXT 900640,'Forecast Level',30,10,null,null,50
exec SP_MID_SET_TEXT 900641,'OR',30,10,null,null,50
/* End TT#274 */
/* Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method   */
exec SP_MID_SET_TEXT 900642,'Average Pack Deviation Tolerance',50,1,null,null,0
exec SP_MID_SET_TEXT 900643,'Maximum Pack Allocation Need Tolerance',50,1,null,null,0
/* Begin TT#356 */
/* Begin TT#384 - JSmith - Error removing child from hierarchy */
exec SP_MID_SET_TEXT 900644,'Auto Refresh',30,10,null,null,50
/* End TT#384 */

/* Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method   */
exec SP_MID_SET_TEXT 900642,'Average Pack Deviation Tolerance',50,1,null,null,0
exec SP_MID_SET_TEXT 900643,'Maximum Pack Allocation Need Tolerance',50,1,null,null,0
/* Begin TT#356 */
/* Begin TT#384 - JSmith - Error removing child from hierarchy */
exec SP_MID_SET_TEXT 900644,'Auto Refresh',30,10,null,null,50
/* End TT#384 */

/* Begin TT#391 - STodd - Size Day to Week Summary */
exec SP_MID_SET_TEXT 900645,'Size Day To Week Summary',30,0,null,null,50
exec SP_MID_SET_TEXT 900646,'Date Range To Process:',30,0,null,null,50
/* End TT#391 - STodd - Size Day to Week Summary */
/* Begin TT#155 - JScott - Add Size Curve info to Node Properties */
exec SP_MID_SET_TEXT 900647,'Highest Level Sales Tolerance',30,10,null,null,50
exec SP_MID_SET_TEXT 900648,'Minimum Average per Size',30,10,null,null,50
exec SP_MID_SET_TEXT 900649,'Highest Level',30,10,null,null,50
--Begin TT#1076 - JScott - Size Curves by Set
--exec SP_MID_SET_TEXT 900650,'Apply Chain Sales',30,10,null,null,50
exec SP_MID_SET_TEXT 900650,'Apply Chain/Set Sales',30,10,null,null,50
--End TT#1076 - JScott - Size Curves by Set
exec SP_MID_SET_TEXT 900651,'Sales Tolerance',30,10,null,null,50
exec SP_MID_SET_TEXT 900652,'Index to Average',30,10,null,null,50
exec SP_MID_SET_TEXT 900653,'Units',30,10,null,null,50
exec SP_MID_SET_TEXT 900654,'Min/Max Tolerance %',30,10,null,null,50
exec SP_MID_SET_TEXT 900655,'Minimum %',30,10,null,null,50
exec SP_MID_SET_TEXT 900656,'Maximum %',30,10,null,null,50
/* End TT#155 - JScott - Add Size Curve info to Node Properties */

/* Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings. */
exec SP_MID_SET_TEXT 900657,'Generic Size Curve Name',30,10,null,null,50
exec SP_MID_SET_TEXT 900658,'Use',30,10,null,null,50
exec SP_MID_SET_TEXT 900659,'Node Properties Name',30,10,null,null,50
exec SP_MID_SET_TEXT 900660,'Header Characteristics',30,10,null,null,50
exec SP_MID_SET_TEXT 900661,'Use Default',30,10,null,null,50
/* End TT#413 */
/* Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings. */
exec SP_MID_SET_TEXT 900662,'< No Name Extension specified >',250,0,5,null,0	
/* End TT#413 */
/* Begin TT#391 - stodd - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings. */
exec SP_MID_SET_TEXT 900663,'Generate Size Curves Using',250,0,5,null,0	
/* End TT#391 */
/* Begin TT#450 - JScott - SZ CURVE- node properties->tolerance->minimum average per size - store 1101 should get style curve, instead getting style/color chain curve - not correct */
exec SP_MID_SET_TEXT 900664,'< No Higher Level >',250,0,5,null,0
/* End TT#450 - JScott - SZ CURVE- node properties->tolerance->minimum average per size - store 1101 should get style curve, instead getting style/color chain curve - not correct */
/* Begin TT#391 - STodd - Size Day to Week Summary */
exec SP_MID_SET_TEXT 900665,'Override Merchandise:',30,0,null,null,50
/* End TT#391 - STodd - Size Day to Week Summary */
/* Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name */
exec SP_MID_SET_TEXT 900666,':ID=',30,1,null,null,50 
/* End TT#234 */
/* Begin TT#498 - RMatelic - Size Need Use Default warning and Add rule issues  */
exec SP_MID_SET_TEXT 900667,'and Basis Substitute',30,1,null,null,0 
/* End TT#498  */
/* Begin TT#483 - JScott - Add Size Lost Sales criteria and processing */
exec SP_MID_SET_TEXT 900668,'Out of Stock',30,1,null,null,0
exec SP_MID_SET_TEXT 900669,'Sell Thru',30,1,null,null,0
exec SP_MID_SET_TEXT 900670,'Sell Thru Limit',30,1,null,null,0
/* End TT#483 - JScott - Add Size Lost Sales criteria and processing */
/* BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67) */
exec SP_MID_SET_TEXT 900671,'Generic Pack Rounding',30,0,null,null,0
exec SP_MID_SET_TEXT 900672,'1st Pack - Round up from',30,0,null,null,0
exec SP_MID_SET_TEXT 900673,'Nth Pack - Round up from',30,0,null,null,0
exec SP_MID_SET_TEXT 900674,'Pack Multiple',30,0,null,null,0
exec SP_MID_SET_TEXT 900675,'Pack Rounding',30,0,null,null,0
exec SP_MID_SET_TEXT 900676,'1st Pack %',30,0,null,null,0
exec SP_MID_SET_TEXT 900677,'Nth Pack %',30,0,null,null,0
exec SP_MID_SET_TEXT 900678,'All Generic Packs',30,10,null,null,0
exec SP_MID_SET_TEXT 900679,'Ship Up To',30,10,null,null,0
exec SP_MID_SET_TEXT 900693,'Apply Pack Rounding to Generic Packs Only\nRound Up From %',30,0,null,null,0
/* END TT#616 - AGallagher - Allocation - Pack Rounding (#67) */
/* BEGIN TT#619 - AGallagher – OTS Forecast – Chain Plan not required (#46) */
exec SP_MID_SET_TEXT 900680,'Trend Options',30,0,null,null,0
exec SP_MID_SET_TEXT 900681,'Chain Plans',30,0,null,null,0
exec SP_MID_SET_TEXT 900682,'Chain WOS',30,0,null,null,0
exec SP_MID_SET_TEXT 900683,'Plug Chain WOS',30,0,null,null,0
/* END TT#619 - AGallagher – OTS Forecast – Chain Plan not required (#46) */
/* BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) */
exec SP_MID_SET_TEXT 900684,'Matrix Mode',30,0,null,null,0
exec SP_MID_SET_TEXT 900685,'Normal',30,0,null,null,0
exec SP_MID_SET_TEXT 900686,'Average',30,0,null,null,0
exec SP_MID_SET_TEXT 900687,'Spread Option',30,0,null,null,0
exec SP_MID_SET_TEXT 900688,'Spread by Index',30,0,null,null,0
exec SP_MID_SET_TEXT 900689,'Smooth',30,0,null,null,0
/* END TT#637 - AGallagher - Velocity - Spread Average (#7) */
/* BEGIN TT#667 - Stodd - Pre-allocate Reserve */
exec SP_MID_SET_TEXT 900690,'Reserve As Bulk',30,0,null,null,0
exec SP_MID_SET_TEXT 900691,'Reserve As Packs',30,0,null,null,0
/* END TT#667 - Stodd - Pre-allocate Reserve */
/* Begin TT#707 - JScott - Size Curve process needs to multi-thread */
exec SP_MID_SET_TEXT 900692,'Concurrent Processes',30,0,null,null,0
/* End TT#707 - JScott - Size Curve process needs to multi-thread */

/* BEGIN TT#1287 - AGallagher - Inventory Min/Max */
exec SP_MID_SET_TEXT 900694,'Allocation Min/Max',30,0,null,null,0
exec SP_MID_SET_TEXT 900695,'Inventory Min/Max',30,0,null,null,0
exec SP_MID_SET_TEXT 900696,'Inventory Basis',30,0,null,null,0
exec SP_MID_SET_TEXT 900697,'Min/Max Options',30,0,null,null,0
/* END TT#1287 - AGallagher - Inventory Min/Max */
exec SP_MID_SET_TEXT 900698,'Apply Rules Only',30,0,null,null,0
exec SP_MID_SET_TEXT 900699,'Expand Headers When Intransit is Relieved',30,0,null,null,0  /*TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved */


/* BEGIN RELEASE 3.2.0.20 02-24-2010 -APicchetti - TT#370 - Build Packs Enhancement */
exec SP_MID_SET_TEXT  900700, 'Method',50,0,null,null,0
exec SP_MID_SET_TEXT  900701, 'Criteria',50,0,null,null,0
exec SP_MID_SET_TEXT  900702, 'Constraints',50,0,null,null,0
exec SP_MID_SET_TEXT  900703, 'Summary',50,0,null,null,0
exec SP_MID_SET_TEXT  900704, 'Evaluation',50,0,null,null,0
exec SP_MID_SET_TEXT  900705, 'Criteria',50,0,null,null,0
exec SP_MID_SET_TEXT  900706, 'Header',50,0,null,null,0
exec SP_MID_SET_TEXT  900707, 'Get Header Sizes',50,0,null,null,0     /* tt#911 Changed the button text from "Get Header" to "Get Header Sizes" - apicchetti 10/5/2010 */
exec SP_MID_SET_TEXT  900708, 'Vendor Pack Minimum',50,0,null,null,0  /* TT#787 Vendor Min Order applies only to packs */
exec SP_MID_SET_TEXT  900709, 'Vendor',50,0,null,null,0
exec SP_MID_SET_TEXT  900710, 'Size Multiple',50,0,null,null,0
exec SP_MID_SET_TEXT  900711, 'Size Group',50,0,null,null,0
exec SP_MID_SET_TEXT  900712, 'Size Curve',50,0,null,null,0
exec SP_MID_SET_TEXT  900713, 'Pack Combinations',50,0,null,null,0
exec SP_MID_SET_TEXT  900714, 'Reserve',50,0,null,null,0
exec SP_MID_SET_TEXT  900715, 'Reserve As Bulk',50,0,null,null,0
exec SP_MID_SET_TEXT  900716, 'Reserve As Packs',50,0,null,null,0
exec SP_MID_SET_TEXT  900717, 'Pack Error Options',50,0,null,null,0
exec SP_MID_SET_TEXT  900718, 'Average Pack Deviation Tolerance',50,0,null,null,0
exec SP_MID_SET_TEXT  900719, 'Ship Variance',50,0,null,null,0
exec SP_MID_SET_TEXT  900720, 'Pack Options',50,0,null,null,0
exec SP_MID_SET_TEXT  900721, 'Pack Combination',50,0,null,null,0
exec SP_MID_SET_TEXT  900722, 'Ship Variance',50,0,null,null,0
exec SP_MID_SET_TEXT  900723, 'Maximum Pack Need Tolerance',50,0,null,null,0
exec SP_MID_SET_TEXT  900724, 'Average Pack Deviation Tolerance',50,0,null,null,0
exec SP_MID_SET_TEXT  900725, 'All Store Total Buy',50,0,null,null,0
exec SP_MID_SET_TEXT  900726, 'All Store Bulk Buy',50,0,null,null,0
exec SP_MID_SET_TEXT  900727, 'Non-reserve Total Buy',50,0,null,null,0
exec SP_MID_SET_TEXT  900728, 'Non-reserve Bulk Buy',50,0,null,null,0
exec SP_MID_SET_TEXT  900729, 'Reserve Total Buy',50,0,null,null,0
exec SP_MID_SET_TEXT  900730, 'Reserve Bulk Buy',50,0,null,null,0
exec SP_MID_SET_TEXT  900731, 'All Store Total Pack Units',50,0,null,null,0
exec SP_MID_SET_TEXT  900732, 'Non-reserve Total Pack Units',50,0,null,null,0
exec SP_MID_SET_TEXT  900733, 'Reserve Total Pack Units',50,0,null,null,0
exec SP_MID_SET_TEXT  900734, 'All Store Total Number Of Packs',50,0,null,null,0
exec SP_MID_SET_TEXT  900735, 'Non-reserve Total Number Of Packs',50,0,null,null,0
exec SP_MID_SET_TEXT  900736, 'Reserve Total Number Of Packs',50,0,null,null,0
exec SP_MID_SET_TEXT  900737, 'Sizes With Units Count',50,0,null,null,0
exec SP_MID_SET_TEXT  900738, 'Sizes With At Least One Error Count',50,0,null,null,0
exec SP_MID_SET_TEXT  900739, 'Total Size Unit Error',50,0,null,null,0
exec SP_MID_SET_TEXT  900740, 'Average Error Per Size With Units',50,0,null,null,0
exec SP_MID_SET_TEXT  900741, 'Average Error Per Size In Error',50,0,null,null,0
exec SP_MID_SET_TEXT  900742, 'Average Error Per Pack',50,0,null,null,0
exec SP_MID_SET_TEXT  900743, 'Average Error Per Pack With Error',50,0,null,null,0
exec SP_MID_SET_TEXT  900744, 'Average Error Per Store With Error',50,0,null,null,0
exec SP_MID_SET_TEXT  900745, 'Average Error Per Store With Packs',50,0,null,null,0
exec SP_MID_SET_TEXT  900746, 'Average Error Per Store With Units',50,0,null,null,0
exec SP_MID_SET_TEXT  900747, 'Percent All Store Units In Packs',50,0,null,null,0
exec SP_MID_SET_TEXT  900748, 'Percent of Non-reserve Units In Packs',50,0,null,null,0
exec SP_MID_SET_TEXT  900749, 'Percent of Reserve Units In Packs',50,0,null,null,0
exec SP_MID_SET_TEXT  900750, 'Percent of Reserve In Total',50,0,null,null,0
exec SP_MID_SET_TEXT  900751, 'Percent of Bulk To Total',50,0,null,null,0
exec SP_MID_SET_TEXT  900752, 'Percent of Bulk In Reserve',50,0,null,null,0
exec SP_MID_SET_TEXT  900753, 'Percent of Bulk Total In Reserve',50,0,null,null,0
exec SP_MID_SET_TEXT  900754, 'Percent of Reserve In Bulk',50,0,null,null,0
exec SP_MID_SET_TEXT  900755, 'Count of All Stores With Packs',50,0,null,null,0
exec SP_MID_SET_TEXT  900756, 'Count of Non-reserve Stores With Packs',50,0,null,null,0
exec SP_MID_SET_TEXT  900757, 'Count of All Stores With Units',50,0,null,null,0
exec SP_MID_SET_TEXT  900758, 'Count of Non-reserve Stores With Units',50,0,null,null,0
exec SP_MID_SET_TEXT  900759, 'Count of All Stores With Bulk',50,0,null,null,0
exec SP_MID_SET_TEXT  900760, 'Count of Non-reserve Stores With Bulk',50,0,null,null,0
exec SP_MID_SET_TEXT  900761, 'Stores With Error Count',50,0,null,null,0
exec SP_MID_SET_TEXT  900762, 'Percent of Non-reserve With Units In Error',50,0,null,null,0
exec SP_MID_SET_TEXT  900763, 'Key',50,0,null,null,0
go
/* END RELEASE 3.2.0.20 02-24-2010 -APicchetti - TT#370 - Build Packs Enhancement */

/* BEGIN TT#669 - AGallagher – Build Pack Method – Variance Options */
exec SP_MID_SET_TEXT 900764,'Variance Options',30,0,null,null,0
exec SP_MID_SET_TEXT 900765,'Deplete Reserve',30,0,null,null,0
exec SP_MID_SET_TEXT 900766,'Increase Buy Quantity',30,0,null,null,0
exec SP_MID_SET_TEXT 900767,'%',30,0,null,null,0
/* END TT#669 - AGallagher – Build Pack Method – Variance Options */
--Begin TT#812 - JScott - Missing text on the About Dialog for an new install
exec SP_MID_SET_TEXT 900768,'User',30,0,null,null,50
exec SP_MID_SET_TEXT 900769,'Version',30,0,null,null,50
exec SP_MID_SET_TEXT 900770,'Environment',30,0,null,null,50
exec SP_MID_SET_TEXT 900771,'Last updated on: {0} at {1}',30,0,null,null,50
exec SP_MID_SET_TEXT 900772,'Operating System',30,0,null,null,50
exec SP_MID_SET_TEXT 900773,'Edition',30,0,null,null,50
exec SP_MID_SET_TEXT 900774,'Service Pack',30,0,null,null,50
exec SP_MID_SET_TEXT 900775,'Microsoft .Net Framework Version',30,0,null,null,50
exec SP_MID_SET_TEXT 900776,'Unavailable',30,0,null,null,50
--End TT#812 - JScott - Missing text on the About Dialog for an new install
exec SP_MID_SET_TEXT 900777,'Remove Bulk From Header',30,0,null,null,0 /*TT#744 - Use Orig Pack Fit Logic; Option No Bulk */
exec SP_MID_SET_TEXT 900778,'Original Buy Packaged Units',50,0,null,null,0 /*TT#801 - BP Need additional Pack Selection Criteria */
exec SP_MID_SET_TEXT 900779,'Percent Original Buy Packaged',50,0,null,null,0 /*TT#801 - BP Need additional Pack Selection Criteria */
/* Begin TT#2 - RMatelic -assortment */
exec SP_MID_SET_TEXT 900780,'Empty Row',30,0,null,null,0
exec SP_MID_SET_TEXT 900781,'Color Browser',30,0,null,null,0
/* End TT#2  */
/* Begin TT#2 - stodd -assortment */
exec SP_MID_SET_TEXT 900782,'Database',30,0,null,null,0
/* End TT#2 - stodd -assortment */

/* Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model */
exec SP_MID_SET_TEXT 900783,'Active Only',30,0,null,null,50
exec SP_MID_SET_TEXT 900784,'Active',30,0,null,null,50
exec SP_MID_SET_TEXT 900785,'Inactive',30,0,null,null,50
/* End TT#988 */
exec SP_MID_SET_TEXT 900786,'S',10,1,2,0,0
--Begin TT#1076 - JScott - Size Curves by Set
--Begin TT#1347 - JScott - Size Curve by Set not giving correct results
--exec SP_MID_SET_TEXT 900787,'Store',50,0,null,null,0
exec SP_MID_SET_TEXT 900787,'Chain',50,0,null,null,0
--End TT#1347 - JScott - Size Curve by Set not giving correct results
exec SP_MID_SET_TEXT 900788,'Set',50,0,null,null,0
--End TT#1076 - JScott - Size Curves by Set
exec SP_MID_SET_TEXT 900789,'Reconcile',30,0, null,null,0 /* TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance */
/* BEGIN TT#1135 - AGallagher – Export headers from allocation workspace */
exec SP_MID_SET_TEXT 900790,'Export',10,0, null,null,0
exec SP_MID_SET_TEXT 900791,'{0} Workspace Explorer Header(s) Export to Excel',50,0, null,null,0
exec SP_MID_SET_TEXT 900792,'Please Select Type of Export to Excel',50,0, null,null,0
exec SP_MID_SET_TEXT 900793,'Summary - All Header(s)',50,0, null,null,0
exec SP_MID_SET_TEXT 900794,'Summary - Selected Header(s)',50,0, null,null,0
exec SP_MID_SET_TEXT 900795,'Detail - Selected Header(s)',50,0, null,null,0
exec SP_MID_SET_TEXT 900796,'No Header(s) were selected to export to Excel',50,0, null,null,0
exec SP_MID_SET_TEXT 900797,'All Header(s) successfully exported to\n',50,0, null,null,0
exec SP_MID_SET_TEXT 900798,'Selected Header(s) successfully exported to\n',50,0, null,null,0
exec SP_MID_SET_TEXT 900799,'Detail Selected Header(s) successfully exported to\n',50,0, null,null,0
exec SP_MID_SET_TEXT 900800,'\n\nNumber of Header(s) exported to Excel: ',50,0, null,null,0
/* END TT#1135 - AGallagher – Export headers from allocation workspace */
exec SP_MID_SET_TEXT 900801,'Content',50,0, null,null,0   /* TT#4900 - JSmith - GA- open Group and the Content tab says 'Bad or Missing text code:900801 */
exec SP_MID_SET_TEXT 900802,'Refresh',50,0, null,null,0
/* Begin TT#1227 - stodd – sort seq */
exec SP_MID_SET_TEXT 900803,'Placeholder Seq',50,0, null,null,0
exec SP_MID_SET_TEXT 900804,'Header Seq',50,0, null,null,0
/* End TT#1227 - stodd – sort seq */
/* BEGIN TT#1401 - stodd - add resevation stores (IMO)	*/
exec SP_MID_SET_TEXT 900805,'VSW',50,9,12,0,0
exec SP_MID_SET_TEXT 900806,'Min Ship Qty',30,1,2,null,0
exec SP_MID_SET_TEXT 900807,'% Pack Threshold',30,1,2,null,0
exec SP_MID_SET_TEXT 900808,'Item Max',30,1,2,null,0
exec SP_MID_SET_TEXT 900809,'Push to Backstock',30,1,2,null,0
/* END TT#1401 - stodd - add resevation stores (IMO)	*/
/* Begin TT#1401 - RMatelic - Reservation Stores   */
/* BEGIN TT#3475 - agallagher - Add the Store Allocated Units and VSW Allocated Units breakdown to the Allocation Workspace */
/* exec SP_MID_SET_TEXT 900810,'VSW Units Allocated',30,1,2,null,0 */
/* exec SP_MID_SET_TEXT 900811,'VSW Orig Units Allocated',30,1,2,null,0 */
exec SP_MID_SET_TEXT 900810,'Store Allocated Units',30,1,2,null,0
exec SP_MID_SET_TEXT 900811,'VSW Allocated Units',30,1,2,null,0
/* End TT#3475   */
/* exec SP_MID_SET_TEXT 900812,'VSW',50,9,12,null,0  TT#1401 - GTaylor */
exec SP_MID_SET_TEXT 900812,'VSW',30,1,0,null,0 /* TT#2225 - AGallagher - VSW Modifcations Enhancement */
/* End TT#1401   */

/* Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue */
exec SP_MID_SET_TEXT 900813,' Apply Minimum to Zero Tolerance',50,1,2,0,0
/* End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue */
exec SP_MID_SET_TEXT 900814,' Chain Set %',50,1,2,0,0  /* TT#2138 - JSmith - Open Node Properties Read Only */
/* BEGIN TT#2153 - stodd - Assortment Export */
exec SP_MID_SET_TEXT 900815,'Assortment Content Grid Header(s) Export to Excel',100,0,null,null,0
/* BEGIN TT#2153 - stodd - Assortment Export  */
/*BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement */
exec SP_MID_SET_TEXT 900819,'Project Current Week Sales',30,1,0,0,0
/*END TT#43 - MD - DOConnell - Projected Sales Enhancement */

/*BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement */
exec SP_MID_SET_TEXT 900820,'Stock Lead Weeks',30,1,0,0,0
/*BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement */
/*BEGIN TT#46 - -jsobek –Develop My Activity Log */ 
exec SP_MID_SET_TEXT 900821,'Clear',50,1,2,0,0
exec SP_MID_SET_TEXT 900822,'Time',50,1,2,0,0
exec SP_MID_SET_TEXT 900823,'Module',50,1,2,0,0
exec SP_MID_SET_TEXT 900824,'Message Level',50,1,2,0,0
exec SP_MID_SET_TEXT 900825,'Message',50,1,2,0,0
exec SP_MID_SET_TEXT 900826,'Message Details',50,1,2,0,0
exec SP_MID_SET_TEXT 900827,'Message Breakdown',50,1,2,0,0
exec SP_MID_SET_TEXT 900828,'Show Chart',50,1,2,0,0
/* End TT#46 MD */
exec SP_MID_SET_TEXT 900816,'VSW Process',50,1,0,null,0 /* TT#2225 – Jellis – AnF VSW FWOS Enhancement pt 1 */
exec SP_MID_SET_TEXT 900817,'FWOS Max',250,0,2,null,0 /* TT#2225 – GTaylor – ANF VSW */
exec SP_MID_SET_TEXT 900818,'FWOS Max must have a value.',250,0,2,null,0 /* TT#2225 – GTaylor – ANF VSW */
/*BEGIN TT#2576 - DOConnell - Global Lock Create - Title Bar Name Changes */ 
exec SP_MID_SET_TEXT 900832,'Lock',30,10,null,null,50
/*End TT#2576 - DOConnell - Global Lock Create - Title Bar Name Changes */ 
/*BEGIN TT#292-MD - JSmith - Enhanced Help - About */ 
exec SP_MID_SET_TEXT 900833,'Web Page',30,10,null,null,50
exec SP_MID_SET_TEXT 900834,'Support Page',30,10,null,null,50
/*End TT#292-MD - JSmith - Enhanced Help - About */
exec SP_MID_SET_TEXT 900835,'VSW Size Constraints',30,0,null,null,0 /* TT#246-MD – AGallagher – VSW Size */
exec SP_MID_SET_TEXT 900836,'Override Default',30,0,null,null,0 /* TT#246-MD – AGallagher – VSW Size  */
/*BEGIN TT#46 - -jsobek –Develop My Activity Log */ 
exec [dbo].[SP_MID_SET_TEXT] 900837,'This will permanently remove all messages.\n Are you sure you wish to continue??',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900838,'Confirm:',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900839,'Message limit cannot be greater than {0}.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900840,'Enter a valid number for the max message limit.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900841,'Invalid Max Message Limit',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900842,'Message limit cannot be less than 50.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900843,'Restoring original value.',250,0,0,0,0
/*END TT#46 -jsobek –Develop My Activity Log */ 
/*BEGIN TT#506 - -jsobek –Add infrastructure to allow Email to be sent from the application */ 
exec [dbo].[SP_MID_SET_TEXT] 900844,'Email From Address required.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900845,'Email From Address is invalid.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900846,'Email To Address is required.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900847,'Email To Address is invalid.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900848,'Email CC Address is invalid.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900849,'Email BCC Address is invalid.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900850,'Email Subject is required.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900851,'Email Body is required.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900852,'Email Attachments are invalid.\n Details:\n{0}',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900853,'Email SMTP settings are not configured.',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900854,'Problem connecting to SMTP server.\n Details:\n{0}',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900855,'Problem sending email.\n Details:\n{0}',250,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900856,'Email message was sent successfully to MID Retail.',250,0,0,0,0

/* BEGIN TT#708-MD - Stodd - Group Allocation Prototype. */
/* exec [dbo].[SP_MID_SET_TEXT] 900882,'Group Min',30,0,0,0,0 */   /* TT#937 - MD - stodd - GA method missing 'group min' column' */
exec [dbo].[SP_MID_SET_TEXT] 900860,'Group Max',30,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900861,'Header Min',30,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900862,'Header Max',30,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900863,'Line Item Minimum Override',50,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900864,'Group Allocation',50,0,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900865,'Group',50,0,0,0,0
/* END TT#708-MD - Stodd - Group Allocation Prototype. */
exec SP_MID_SET_TEXT 900866,'All Header(s)',50,0, null,null,0
exec SP_MID_SET_TEXT 900867,'Selected Header(s)',50,0, null,null,0
exec SP_MID_SET_TEXT 900868,'Marked for Deletion',50,9,12,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900869,'Store Delete Settings',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */ 
exec SP_MID_SET_TEXT 900870,'Number of Concurrent Processes for Copy/Loads',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900871,'Number of Concurrent Processes for Deletes',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900872,'Batch Size',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900873,'Minimum Row Count',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900874,'Maximum Row Count',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900875,'Row Percentage Maximum',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900876,'Analysis',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900877,'Process Analysis',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900878,'Messages',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900879,'Process Store &Delete',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900880,'Reset for Restart',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */
exec SP_MID_SET_TEXT 900881,'Store Delete',50,9,0,0,0	/* TT#739-MD - STodd - delete stores */

exec SP_MID_SET_TEXT 900882,'Header Balance',50,9,0,0,0	/* TT#855-MD -jsobek -Velocity Enhancements */

exec SP_MID_SET_TEXT 900883,'Group AllocationID',50,0,0,0,0	/* TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace */
exec [dbo].[SP_MID_SET_TEXT] 900888,'Group Min',30,0,0,0,0 /* TT#937 - MD - stodd - GA method missing 'group min' column' */
/* BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis */
exec SP_MID_SET_TEXT 900889,'Group Min/Max Options',30,0,null,null,0
exec SP_MID_SET_TEXT 900890,'Header Min/Max Options',30,0,null,null,0
/* END TT#1287 - AGallagher - Add Header Min/Max Options and Inventory Basis */

/* Begin TT#1126-MD -RMatelic -  Show Details needs consistent “controls” as the main Workspace Explorer */
exec SP_MID_SET_TEXT 900891,'Grid',30,1,2,0,0 
/* End TT#1126-MD */
/* Begin TT#3859 - stodd - Balance message text */
exec SP_MID_SET_TEXT 900896,'Header Total',30,1,0,0,0 
exec SP_MID_SET_TEXT 900897,'Placeholder Total',30,1,0,0,0 
/* End TT#3859 - stodd - Balance message text */
exec SP_MID_SET_TEXT 900898,'Unspecified',30,1,2,0,0 /* TT#1378-MD - RMatelic  -Add soft text label for Unspecified Size Group */
exec SP_MID_SET_TEXT 900899,'Show Sign Off Prompt',30,1,2,0,0 /* TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner */

/*Begin TT#901-MD -jsobek –BatchOnlyMode */
exec [dbo].[SP_MID_SET_TEXT] 900900,'Shutdown command issued from control service: ',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900901,'Message From Control Service:',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900902,'Batch Only Mode turned on: ',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900903,'Batch Only Mode turned off: ',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900904,'Running Local - No system functions are currently available.',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900905,'Not connected to control service - No system functions are currently available.',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900906,'Log Out command has been sent to selected users.',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900907,'Batch Only Mode is ON',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900908,'Batch Only Mode is OFF',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900909,'Batch Only Mode has already been turned ON by: ',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900910,'Batch Only Mode has already been turned OFF by: ',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900911,'Batch Mode will be turned ON.  Please wait while client applications shut down.',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900912,'Batch Mode will be turned OFF.',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900913,'Message has been sent to clients.',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900914,' user(s) logged in',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900915,'User',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900916,'Machine',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900917,'IP Address',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900918,'Login Options',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900919,'Use Windows Authentication',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900920,'Enforce Single Instance',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900921,'Enforce Single User Login',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900922,'Enable Remote System Options',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900923,'Start with Batch Only Mode on',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900924,'Turn On',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900925,'Turn Off',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900926,'Send Msg',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900927,'Message for clients:',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900928,'Last Changed:',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900929,'Current Users',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900930,'Show Current Users',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900931,'Log Out Selected Users',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900932,'Please select one or more users to log out.',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900933,'Please type a message to send.',250,0,0,0,0 
/*End TT#901-MD -jsobek –BatchOnlyMode */ 
/*Begin TT#1349-MD -jsobek -Store Filters - Merchandise Selector - add new "No Size" option to prevent selection sizes */
exec [dbo].[SP_MID_SET_TEXT] 900934,'Invalid Merchandise Node',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900935,'Selected Merchandise must be at the Style Level.',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900936,'Selected Merchandise must be at the Color Level.',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900937,'Selected Merchandise must be at the Size Level.',250,0,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900938,'Selected Merchandise cannot be at the Size Level.',250,0,0,0,0 
/*End TT#1349-MD -jsobek -Store Filters - Merchandise Selector - add new "No Size" option to prevent selection sizes */
exec [dbo].[SP_MID_SET_TEXT] 900939,'Logility - RO',30,1,0,0,0  /* TT#4287 - JSmith - Do not allow refresh if windows are open */
/* Begin TT#1581-MD - stodd - API header reconcile */
exec [dbo].[SP_MID_SET_TEXT] 900940,'Transaction Files Read',30,1,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900941,'Records Read',30,1,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900942,'Duplicate Records Found',30,1,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900943,'Records Skipped (includes duplicates found)',50,1,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900944,'Records Written',30,1,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900945,'Transaction Files Written',30,1,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900946,'Remove Records Written',30,1,0,0,0 
exec [dbo].[SP_MID_SET_TEXT] 900947,'Remove Transaction Files Written',50,1,0,0,0  
/* End TT#1581-MD - stodd - API header reconcile */
exec [dbo].[SP_MID_SET_TEXT] 900948,'All',30,102,0,0,0	/* TT#1595-MD - stodd - Batch comp */
exec [dbo].[SP_MID_SET_TEXT] 900951,'Prior Header - Include Reserve',50,0,0,0,0	/* TT#1609-MD - stodd - prior header */
exec [dbo].[SP_MID_SET_TEXT] 900952,'Include Reserve',30,0,0,0,0	/* TT#1609-MD - stodd - prior header */
/* Begin TT#1652-MD - RMatelic - DC Carton Rounding */
exec SP_MID_SET_TEXT 900955,'DC Carton Rounding',30,1,0,0,0		   
exec SP_MID_SET_TEXT 900956,'Apply Overage To:',30,1,0,0,0		   
exec SP_MID_SET_TEXT 900957,'Allocated Stores',30,1,0,0,0  
exec SP_MID_SET_TEXT 900958,'Apply DC Carton Rounding for Bulk and/or Generic Packs Only',70,1,0,0,0  
/* End TT#1652-MD  */
/* Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields   */
exec SP_MID_SET_TEXT 900959,'Refresh to see changes',70,1,0,0,0 
/* End TT#1844-MD  */
/* Begin TT#1966-MD - JSmith- DC Fulfillment   */
exec SP_MID_SET_TEXT 900960,'Create Master Headers for same Style/Color Headers within the Filtered List of Headers',70,1,0,0,0 
exec SP_MID_SET_TEXT 900961,'Apply DC Fulfillment to align the stores to their optimal distribution center',70,1,0,0,0 
exec SP_MID_SET_TEXT 900962,'Use Selected Headers (from Allocation Workspace)',70,1,0,0,0 
exec SP_MID_SET_TEXT 900963,'Create Master Headers',70,1,0,0,0 
exec SP_MID_SET_TEXT 900964,'Merchandise',70,1,0,0,0 
exec SP_MID_SET_TEXT 900965,'Filter',70,1,0,0,0 
exec SP_MID_SET_TEXT 900966,'Filter',70,1,0,0,0 
exec SP_MID_SET_TEXT 900967,'Header Filter Overrides',70,1,0,0,0 
exec SP_MID_SET_TEXT 900968,'Header Field/Characteristics',70,1,0,0,0 
exec SP_MID_SET_TEXT 900969,'Value',70,1,0,0,0 
exec SP_MID_SET_TEXT 900970,'Characteristic',70,1,0,0,0 

exec SP_MID_SET_TEXT 900971,'Master Split Options',70,1,0,0,0 
exec SP_MID_SET_TEXT 900972,'DC Fulfillment',70,1,0,0,0 
exec SP_MID_SET_TEXT 900973,'Proportional',70,1,0,0,0 
exec SP_MID_SET_TEXT 900974,'Apply Minimums',70,1,0,0,0 
exec SP_MID_SET_TEXT 900975,'Prioritize Headers By',70,1,0,0,0 
exec SP_MID_SET_TEXT 900976,'Ascending',70,1,0,0,0 
exec SP_MID_SET_TEXT 900977,'Descending',70,1,0,0,0 
exec SP_MID_SET_TEXT 900978,'Order Stores By',70,1,0,0,0 
exec SP_MID_SET_TEXT 900979,'DC',70,1,0,0,0 
exec SP_MID_SET_TEXT 900980,'Store Characteristic',70,1,0,0,0 
exec SP_MID_SET_TEXT 900981,'Ascending',70,1,0,0,0 
exec SP_MID_SET_TEXT 900982,'Descending',70,1,0,0,0 
exec SP_MID_SET_TEXT 900983,'Split by',70,1,0,0,0 
exec SP_MID_SET_TEXT 900984,'Store then DC',70,1,0,0,0 
exec SP_MID_SET_TEXT 900985,'DC then Store',70,1,0,0,0 
exec SP_MID_SET_TEXT 900986,'Reserve',70,1,0,0,0 
exec SP_MID_SET_TEXT 900987,'Pre-split',70,1,0,0,0 
exec SP_MID_SET_TEXT 900988,'Post-split',70,1,0,0,0 
exec SP_MID_SET_TEXT 900989,'Minimums',70,1,0,0,0 
exec SP_MID_SET_TEXT 900990,'Apply when Alloc Qty not available',70,1,0,0,0 
exec SP_MID_SET_TEXT 900991,'Apply first to All Stores',70,1,0,0,0 
exec SP_MID_SET_TEXT 900992,'DC Fulfillment Options',70,1,0,0,0 
exec SP_MID_SET_TEXT 900993,'Within DC',70,1,0,0,0 
exec SP_MID_SET_TEXT 900994,'Fill',70,1,0,0,0 
exec SP_MID_SET_TEXT 900995,'Proportional',70,1,0,0,0 
exec SP_MID_SET_TEXT 900996,'DC Fulfillment Monitor',70,1,0,0,0 
/* End TT#1966-MD - JSmith- DC Fulfillment  */
/* Begin TT#1644-MD - JSmith - Process Control  */
exec SP_MID_SET_TEXT 900997,'Process Control',50,1,0,0,0
exec SP_MID_SET_TEXT 900998,'Process ID',50,1,0,0,0
exec SP_MID_SET_TEXT 900999,'Is Running',50,1,0,0,0
exec SP_MID_SET_TEXT 901000,'Last Modified On',50,1,0,0,0
exec SP_MID_SET_TEXT 901001,'Save Changes',50,1,0,0,0
exec SP_MID_SET_TEXT 901002,'Must Be Running',50,1,0,0,0
exec SP_MID_SET_TEXT 901003,'Cannot Be Running',50,1,0,0,0
exec SP_MID_SET_TEXT 901004,'Services Restart Required',50,1,0,0,0
exec SP_MID_SET_TEXT 901005,'Last Modified By',50,1,0,0,0
/* End TT#1644-MD - JSmith - Process Control  */

/* Begin TT#2131-MD - JSmith - Halo Integration */
exec SP_MID_SET_TEXT 901006,'Low Level',30,10,null,null,50
exec SP_MID_SET_TEXT 901007,'Low Levels Only',30,10,null,null,50
exec SP_MID_SET_TEXT 901008,'Chain Data',30,10,null,null,50
exec SP_MID_SET_TEXT 901009,'Store Data',30,10,null,null,50
exec SP_MID_SET_TEXT 901010,'Attribute Set Data',30,10,null,null,50
exec SP_MID_SET_TEXT 901011,'Attribute',30,10,null,null,50
exec SP_MID_SET_TEXT 901012,'Variables',30,10,null,null,50
exec SP_MID_SET_TEXT 901013,'Total Variables',30,10,null,null,50
exec SP_MID_SET_TEXT 901014,'Concurrent Processes',30,10,null,null,50
exec SP_MID_SET_TEXT 901015,'Exclude Zero Values',30,10,null,null,50
exec SP_MID_SET_TEXT 901016,'Extract Ineligible Stores',30,10,null,null,50
exec SP_MID_SET_TEXT 901017,'Analytics',30,1,2,0,0
/* End TT#2131-MD - JSmith - Halo Integration */
exec SP_MID_SET_TEXT 901018,'Item',50,1,0,0,0  /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 901019,'Client Type',50,1,0,0,0  /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 901020,'Windows',50,1,0,0,0  /* RO-3444 - JSmith - RO Web Messaging */
exec SP_MID_SET_TEXT 901021,'Web',50,1,0,0,0  /* RO-3444 - JSmith - RO Web Messaging */

/* Tool Tips */

exec SP_MID_SET_TEXT 1000000,'Click to add a new basis.',50,69,2,0,0
exec SP_MID_SET_TEXT 1000001,'Click to add new basis details.',50,69,2,0,0
exec SP_MID_SET_TEXT 1000002,'Double click to change folder color',50,69,2,0,0
exec SP_MID_SET_TEXT 1000003,'Click here to show/hide columns',50,69,2,0,0
exec SP_MID_SET_TEXT 1000004,'Click here for header notes',50,69,2,0,0
exec SP_MID_SET_TEXT 1000005,'Click here for Color Browser',50,69,2,0,0

exec SP_MID_SET_TEXT 1000006,'Click to filter drop down list',50,69,2,0,0 
exec SP_MID_SET_TEXT 1000007,'Check to forecast {0}.',50,69,2,0,0
exec SP_MID_SET_TEXT 1000008,'Uncheck to not forecast {0}.',50,69,2,0,0
exec SP_MID_SET_TEXT 1000009,'Check to forecast low levels.',50,69,2,0,0
exec SP_MID_SET_TEXT 1000010,'Uncheck to not forecast low levels.',50,69,2,0,0
exec SP_MID_SET_TEXT 1000011,'Click to add new Stock Min/Max.',50,69,2,0,0
exec SP_MID_SET_TEXT 1000012,'Enter mask of characters and "*" for wildcards',50,69,2,0,0 
exec SP_MID_SET_TEXT 1000013,'Enter the description of the version',50,69,2,0,0
exec SP_MID_SET_TEXT 1000014,'Check if version is to be used',50,69,2,0,0
exec SP_MID_SET_TEXT 1000015,'Check if values prior to the current date are to be protected',50,69,2,0,0
exec SP_MID_SET_TEXT 1000016,'Check if similar store values are to be used in a basis',50,69,2,0,0
exec SP_MID_SET_TEXT 1000017,'Select the blend type of the forecast version',50,69,2,0,0
exec SP_MID_SET_TEXT 1000018,'Select the version to use for history in the forecast version',50,69,2,0,0
exec SP_MID_SET_TEXT 1000019,'Select the version to use for plan in the forecast version',50,69,2,0,0
exec SP_MID_SET_TEXT 1000020,'Check if only monthly values will be used',50,69,2,0,0
exec SP_MID_SET_TEXT 1000022,'Presentation minimum + sales is turned on',50,69,2,0,0
exec SP_MID_SET_TEXT 1000023,'Presentation minimum + sales is turned off',50,69,2,0,0
exec SP_MID_SET_TEXT 1000024,'Presentation minimum + sales will inherit',50,69,2,0,0
exec SP_MID_SET_TEXT 1000025,'Click to add new rollup items.',50,69,2,0,0
/* Begin Track #5395 - JScott - Add ability to discard zero values in Export */
exec SP_MID_SET_TEXT 1000026,'Indicates whether zero values should be omitted from the output',30,10,null,null,50
/* End Track #5395 - JScott - Add ability to discard zero values in Export */

/* InUse Tool */
/* BEGIN TT#110-MD-VStuart - In Use Tool */
exec SP_MID_SET_TEXT 2000000,'Characteristics',50,0,null,null,0
exec SP_MID_SET_TEXT 2000001,'Type',50,0,null,null,0
exec SP_MID_SET_TEXT 2000002,'Attribute',50,0,null,null,0
exec SP_MID_SET_TEXT 2000003,'Attribute Set',50,0,null,null,0
exec SP_MID_SET_TEXT 2000004,'User',50,0,null,null,0
exec SP_MID_SET_TEXT 2000005,'Reference',50,0,null,null,0

exec SP_MID_SET_TEXT 2000006,'Method',50,0,null,null,0
exec SP_MID_SET_TEXT 2000007,'Workflow',50,0,null,null,0
exec SP_MID_SET_TEXT 2000008,'Description',50,0,null,null,0
exec SP_MID_SET_TEXT 2000009,'Tasklist',50,0,null,null,0
exec SP_MID_SET_TEXT 2000010,'Job',50,0,null,null,0

exec SP_MID_SET_TEXT 2000011,'Model',50,0,null,null,0
exec SP_MID_SET_TEXT 2000012,'Node',50,0,null,null,0
exec SP_MID_SET_TEXT 2000013,'Workflow/Tasklist',50,0,null,null,0
exec SP_MID_SET_TEXT 2000014,'Filter',50,0,null,null,0
exec SP_MID_SET_TEXT 2000015,'Meth/User/WkFlow',50,0,null,null,0

exec SP_MID_SET_TEXT 2000016,'Name',50,0,null,null,0
exec SP_MID_SET_TEXT 2000017,'Hierarchy',50,0,null,null,0
exec SP_MID_SET_TEXT 2000018,'Header',50,0,null,null,0
exec SP_MID_SET_TEXT 2000019,'Size Group',50,0,null,null,0
exec SP_MID_SET_TEXT 2000020,'Size Curve Group',50,0,null,null,0
exec SP_MID_SET_TEXT 2000021,'Store',50,0,null,null,0	/* TT#643-MD-VStuart-Need Queries for Store Characteristic Values */
exec SP_MID_SET_TEXT 2000022,'Curve',50,0,null,null,0	/* TT#729-MD-VStuart-In Use 4 Size Curve Criteria with Curve Name in Node Properties */
/* END TT#110-MD-VStuart - In Use Tool */
exec SP_MID_SET_TEXT 2000023,'InUse_By',50,0,null,null,0	/* TT#3509-VStuart-Cannot delete OTS Plan Version */
exec SP_MID_SET_TEXT 2000024,'ProdCharName',50,0,null,null,0	/* TT#4697-VStuart-Investigate Product Characteristic In Use querys-MID */
exec SP_MID_SET_TEXT 2000025,'ProdCharValue',50,0,null,null,0	/* TT#4697-VStuart-Investigate Product Characteristic In Use querys-MID */
/* END TT#110-MD-VStuart - In Use Tool */
/* BEGIN TT#554-MD -jsobek -User Log Level Report */
exec [dbo].[SP_MID_SET_TEXT] 301118,'User Options Review',50,1,12,3,0
exec [dbo].[SP_MID_SET_TEXT] 870550,'User Options Review',50,19,null,null,0 
/* END TT#554-MD -jsobek -User Log Level Report */
/* BEGIN TT#609-MD -jsobek -OTS Forecast Chain Ladder View */
exec SP_MID_SET_TEXT 900857,'Chain-Ladder',50,1,2,0,0 
/* END TT#609-MD -jsobek -OTS Forecast Chain Ladder View */
/* BEGIN TT#639-MD -agallagher - OTS Forecast Totals Rightw */
exec SP_MID_SET_TEXT 900858,'Totals Right',50,1,2,0,0 
/* END TT#639-MD -agallagher - OTS Forecast Totals Right */
/* BEGIN TT#739-MD -jsobek -Delete Stores -Allocation by Store Report */
/*exec [dbo].[SP_MID_SET_TEXT] 301119,'Allocation By Store Report',50,1,12,3,0*/	/* TT#950 - MD - stodd - moved this up to be sequential */	
exec [dbo].[SP_MID_SET_TEXT] 870560,'Allocation By Store Report',50,19,null,null,0 
/* END TT#739-MD -jsobek -Delete Stores -Allocation by Store Report */
/* BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max */
exec [dbo].[SP_MID_SET_TEXT] 900884,'Item/FWOS Max',50,0,null,null,0
exec [dbo].[SP_MID_SET_TEXT] 900885,'Default',50,0,null,null,0
exec [dbo].[SP_MID_SET_TEXT] 900886,'Highest',50,0,null,null,0
exec [dbo].[SP_MID_SET_TEXT] 900887,'Lowest',50,0,null,null,0
/* END TT#933-MD - AGallagher - Item Max vs. FWOS Max */

/* Begin TT#1652-MD - RMatelic - DC Carton Rounding */
exec [dbo].[SP_MID_SET_TEXT] 900953,'DC Carton Rounding Default Attribute',40,1,0,0,0
exec [dbo].[SP_MID_SET_TEXT] 900954,'UnitsPerCarton',30,1,0,0,0
/* End TT#1652-MD - RMatelic - DC Carton Rounding */
go


