*Brief*


the task implements the functionality of collecting applications for the conference from potential speakers.
A speaker logs in on the conference website, goes to his personal account and fills in the application form.


The application form includes the topic and description of the report, speaker's data and contacts for feedback.
Each application is unique and extremely valuable, i.e. applications cannot be lost, which means that all submitted applications must be saved in a long-term storage - in a database.
Speakers, on the other hand, are very busy and in-demand specialists, and may be distracted from the application process by urgent work-related matters.

The application process itself is quite exciting and requires a certain amount of concentration and time. This necessitates some drafting mechanism.
The program committee works with applications in an internal CRM system. The CRM system is able to import new requests on a schedule, requesting data from the last import.
There is also a mail robot that sends a reminder letter for requests that have been in drafts for more than 2 weeks. Before closing of applications, the robot starts sending reminders for all applications older than 2 days. The robot is able to receive data on a schedule by transmitting the date of requests older than the one it is interested in.



The API does the following:


*Application creation:*



	POST /applications
	{
	author: "ddfea950-d878-4bfe-a5d7-e9771e830cbd",
	activity: "Report",
	name: "Новые фичи C# vNext",
	description: "Расскажу что нас ждет в новом релизе!",
	outline: "очень много текста... прямо детальный план доклада!",
	}
	===>
	{
	id: "9c53ea53-a88d-4367-ad8a-281738690412",
	author: "ddfea950-d878-4bfe-a5d7-e9771e830cbd",
	activity: "Report",
	name: "Новые фичи C# vNext",
	description: "Расскажу что нас ждет в новом релизе!",
	outline: "очень много текста... прямо детальный план доклада!",
	}

 *Application editing:*
 

	 PUT /applications/9c53ea53-a88d-4367-ad8a-281738690412
	{
	activity: "Report",
	name: "Новые фичи C# theNextGeneratin",
	description: "Расскажу что нас ждет в новейшем релизе!",
	outline: "еще больше текста...",
	}
	===>
	{
	id: "9c53ea53-a88d-4367-ad8a-281738690412",
	author: "ddfea950-d878-4bfe-a5d7-e9771e830cbd",
	activity: "Report",
	name: "Новые фичи C# theNextGeneratin",
	description: "Расскажу что нас ждет в новейшем релизе!",
	outline: "еще больше текста...",
	}

*Deletion of request:*


	DELETE /applications/9c53ea53-a88d-4367-ad8a-281738690412
	==>
	OK, 200
*sending the application for Submission:*

	POST /applications/9c53ea53-a88d-4367-ad8a-281738690412/submit
	==>
	OK, 200

*Receipt of applications after submission Date:*


	GET /applications?submittedAfter="2024-01-01 23:00:00.00"
	==>
	[
	{
		id: "9c53ea53-a88d-4367-ad8a-281738690412",
		author: "ddfea950-d878-4bfe-a5d7-e9771e830cbd",
		activity: "Report",
		name: "Новые фичи C# theNextGeneratin",
		description: "Расскажу что нас ждет в новейшем релизе!",
		outline: "очень много текста...",
	},
	...
	]


 *Receipt of applications not filed and older than a certain date:*

 	GET /applications?unsubmittedOlder="2024-01-01 23:00:00.00"
	==>
	[
	{
		id: "9c53ea53-a88d-4367-ad8a-281738690412",
		author: "ddfea950-d878-4bfe-a5d7-e9771e830cbd",
		activity: "Report",
		name: "Новые фичи C# theNextGeneratin",
		description: "Расскажу что нас ждет в новейшем релизе!",
		outline: "очень много текста...",
	},
	...
	
*Obtaining the current pending application(unsubmitted) for the specified user:*


	GET /users/ddfea950-d878-4bfe-a5d7-e9771e830cbd/currentapplication
	==>
	{
	id: "9c53ea53-a88d-4367-ad8a-281738690412",
	author: "ddfea950-d878-4bfe-a5d7-e9771e830cbd",
	activity: "Report",
	name: "Новые фичи C# theNextGeneratin",
	description: "Расскажу что нас ждет в новейшем релизе!",
	outline: "очень много текста...",
	}

*Receipt of application by identifier:*

	GET /applications/9c53ea53-a88d-4367-ad8a-281738690412
	==>
	{
	id: "9c53ea53-a88d-4367-ad8a-281738690412",
	author: "ddfea950-d878-4bfe-a5d7-e9771e830cbd",
	activity: "Report",
	name: "Новые фичи C# theNextGeneratin",
	description: "Расскажу что нас ждет в новейшем релизе!",
	outline: "очень много текста...",
	}
*obtain a list of possible activity types:*

	GET /activities
	==>
	[
	{ 
		activity: "Report",
		description: "Доклад, 35-45 минут"
	},
	{ 
		activity: "Masterclass",
		description: "Мастеркласс, 1-2 часа"
	},
	{ 
		activity: "Discussion",
		description: "Дискуссия / круглый стол, 40-50 минут"
	}
	]
 

 Criteria
 
* a user can have only one unsent application (draft application)
*	it is impossible to create an application without specifying the user ID

*	it is impossible to create an application without specifying at least one field other than the user ID

*	it is impossible to edit an application so that the user ID + one more field are not filled in.

*	it is not possible to edit submitted requests

*	it is not possible to cancel / delete requests sent for consideration

*	you cannot delete or edit an application that does not exist

*	only applications with all required fields filled in can be sent for review

*	You cannot send a pending application for review

*	a request to receive both submitted and non-submitted applications at the same time should be considered incorrect.






*How To Use*

* Register with your credentials
* Login with your credentials
* Copy token in the response body.
  
  Example "  eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3MTU2NDE1NTl9.J5NZqGoubeE-5Ai_XR06bu8kDUsSGWrqpluIgQ-HNF19aAAiIRl33-BPLEXijghwkC-vGvCvbYcCqfDsffC6WA "

* Click Authorize and enter " bearer with copied token from response body"

  Example " bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3MTU2NDE1NTl9.J5NZqGoubeE-5Ai_XR06bu8kDUsSGWrqpluIgQ-HNF19aAAiIRl33-BPLEXijghwkC-vGvCvbYcCqfDsffC6WA"

 
