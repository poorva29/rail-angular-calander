
/****** Object:  Table [dbo].[pre_registration]    Script Date: 9/4/2014 6:19:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[pre_registration](
	[prereguserid] [int] NOT NULL,
	[firstname] [varchar](50) NOT NULL,
	[lastname] [varchar](50) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[mobileno] [varchar](15) NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


