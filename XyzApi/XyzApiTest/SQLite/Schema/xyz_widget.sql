-- Script Date: 11/17/2018 3:50 PM  - ErikEJ.SqlCeScripting version 3.5.2.74
SELECT 1;
PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE [Widget] (
  [WidgetId] INTEGER  NOT NULL
, [WidgetName] nvarchar(250)  NULL
, CONSTRAINT [PK_Widget] PRIMARY KEY ([WidgetId])
);
COMMIT;

