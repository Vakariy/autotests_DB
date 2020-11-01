Feature: DB_queries
	In order to massage data bese
	As a administrator of a BD
	I want to send queries like INSERT

	

@update_infoUser
Scenario: Update information of a person
	Given new datas for updating and person for updating is ready
	When I updating information aboun person
	Then Information about person has been edited

@add_person
Scenario: Add a new person in data base
	Given information of a new person is ready
	When we send information about person to data bese
	Then we can find information in data base which we sended

@add_order
Scenario: Add a new order for person
	Given datas for order and person is ready
	When we adding order for person
	Then Order has been created for person

@del_person
Scenario: delete person from data base
	Given information of a new person is ready
	When we send information about person to data bese
	And we select a person for delete
	Then person has been deleted
