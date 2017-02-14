(function () {
	'use strict';

	angular.module('app').controller('EmployeeController', EmployeeController);

	function EmployeeController($http) {
		var vm = this;
		var dataService = $http;

		wireUpPublicEvents(vm);

		vm.employee = {};
		vm.employees = [];
		vm.employee = {
			Id: 0,
			EmployeeName: '',
			Salary: 0,
			TaxRate: 0,
			Contribution: 0,
			StateOfResidence: ''
		};

		const pageMode = {
			LIST: 'List',
			EDIT: 'Edit',
			ADD: 'Add'
		};

		vm.uiState = {
			mode: pageMode.LIST,
			isDetailAreaVisible: false,
			isListAreaVisible: true,
			isSearchAreaVisible: true,
			isValid: true,
			messages: []
		};

		employeeList();

		function addClick() {
			setUIState(pageMode.ADD);
		}

		function cancelClick(employeeForm) {
			employeeForm.$setPristine();
			employeeForm.$valid = true;
			vm.uiState.isValid = true;
			setUIState(pageMode.LIST);
		}

		function editClick(id) {
			employeeGet(id)
			setUIState(pageMode.EDIT);
		}

		function deleteClick(id) {
			if (confirm("Delete this Employee?")) {
				deleteData(id);
			}
		}

		function saveClick(employeeForm) {
			if (employeeForm.$valid) {
				if (validateData()) {
					employeeForm.$setPristine();
					if (vm.uiState.mode === pageMode.ADD) {
						insertData();
					}
					else {
						updateData();
					}
				}
				else {
					employeeForm.$valid = false;
				}
			}
			else {
				vm.uiState.isValid = false;
			}
		}

		function insertData() {
			dataService.post("Employee", vm.employee)
			.then(function (result) {
				vm.employee = result.data;
				vm.employees.push(vm.employee);
				setUIState(pageMode.LIST);
			}, function (error) {
				handleException(error);
			});
		}

		function deleteData(id) {
			dataService.delete("/Employee/" + id)
			.then(function (result) {
				var index = vm.employees.map(function (e) { return e.employeeId; }).indexOf(id);
				vm.employees.splice(index, 1);
				setUIState(pageMode.LIST);
			}, function (error) {
				handleException(error);
			});
		}

		function updateData() {
			dataService.put("/Employee/" + vm.employee.Id, vm.employee)
			.then(function (result) {
				vm.employee = result.data;
				var index = vm.employees.map(function (p)
				{ return p.employeeId; })
				.indexOf(vm.employee.Id);
				vm.employees[index] = vm.employee;
				setUIState(pageMode.LIST);
			}, function (error) {
				handleException(error);
			});
		}

		function addValidationMessage(prop, msg) {
			vm.uiState.messages.push({
				property: prop,
				message: msg
			});
		}

		function validateData() {
			vm.uiState.messages = [];

			if (vm.employee.EmployeeName.
			  toLowerCase().indexOf("hacker") >= 0) {
				addValidationMessage('EmployeeName', 'URL can not contain the word \'hacker\'.');
			}

			vm.uiState.isValid = (vm.uiState.messages.length === 0);

			return vm.uiState.isValid;
		}

		function employeeGet(id) {
			dataService.get("/Employee/" + id)
			.then(function (result) {
				vm.employee = result.data;
			}, function (error) {
				handleException(error);
			});
		}

		function setUIState(state) {
			vm.uiState.mode = state;

			vm.uiState.isDetailAreaVisible =
				(state === pageMode.ADD || state === pageMode.EDIT);
			vm.uiState.isListAreaVisible = (state === pageMode.LIST);
			vm.uiState.isSearchAreaVisible = (state === pageMode.LIST);
		}

		function employeeList() {
			dataService.get("Employees")
			.then(function (result) {
				vm.employees = result.data.employees;
				setUIState(pageMode.LIST);
			},
			function (error) {
				handleException(error);
			});
		}

		function wireUpPublicEvents(vm) {
			vm.addClick = addClick;
			vm.cancelClick = cancelClick;
			vm.editClick = editClick;
			vm.deleteClick = deleteClick;
			vm.saveClick = saveClick;
		}

		function handleException(error) {
			vm.uiState.isValid = false;
			var msg = {
				property: 'Error',
				message: ''
			};

			vm.uiState.messages = [];

			switch (error.status) {
				case 400:   // 'Bad Request'
					// Model state errors
					var errors = error.data.ModelState;
					debugger;

					// Loop through and get all
					// validation errors
					for (var key in errors) {
						for (var i = 0; i < errors[key].length; i++) {
							addValidationMessage(key, errors[key][i]);
						}
					}

					break;
				case 404:  // 'Not Found'
					msg.message = 'The employee you were requesting could not be found';
					vm.uiState.messages.push(msg);

					break;
				case 500:  // 'Internal Error'
					msg.message = error.data.ExceptionMessage;
					vm.uiState.messages.push(msg);

					break;
				default:
					msg.message = 'Status: ' + error.status + ' - Error Message: ' + error.statusText;
					vm.uiState.messages.push(msg);

					break;
			}
		}
	}
})();