
/****** Object:  Table [dbo].[tbl_pre_registration]    Script Date: 9/4/2014 6:30:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbl_pre_registration](
	[prereg_userid] [int] IDENTITY(1,1) NOT NULL,
	[firstname] [varchar](50) NOT NULL,
	[lastname] [varchar](50) NOT NULL,
	[email] [varbinary](50) NOT NULL,
	[mobileno] [varchar](15) NOT NULL,
 CONSTRAINT [PK_tbl_pre_registration] PRIMARY KEY CLUSTERED 
(
	[prereg_userid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


