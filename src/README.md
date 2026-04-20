# Chronos Source

## Integration Events

### Employees

| Event | Type | Exchange | Routing Key | Queue |
|-------|------|----------|-------------|-------|
| EmployeeCreated | publish | chronos_employees_core | employee_created | - |
| EmployeeDeleted | publish | chronos_employees_core | employee_deleted | - |

### Contracts

| Event | Type | Exchange | Routing Key | Queue |
|-------|------|----------|-------------|-------|
| ContractCreated | publish | chronos_contracts_core | contract_created | - |
| EmployeeDeleted | consume | chronos_employees_core | employee_deleted | contracts_employees_deleted |
| TimeLogWaitingForAcceptationCreated | consume | chronos_time-logs_core | time_log_waiting_for_acceptation_created | contracts_time_logs_waiting_for_acceptation_created |

### Time-Logs

| Event | Type | Exchange | Routing Key | Queue |
|-------|------|----------|-------------|-------|
| TimeLogWaitingForAcceptationCreated | publish | chronos_time-logs_core | time_log_waiting_for_acceptation_created | - |

### Notifications

| Event | Type | Exchange | Routing Key | Queue |
|-------|------|----------|-------------|-------|
| EmployeeCreated | consume | chronos_employees_core | employee_created | notifications_employee_created |
| TimeLogCreated | consume | chronos_time_loggers_core | time_log_created | notifications_time_log_created |
| SupervisorAssigned | consume | chronos_employees_core | supervisor_assigned | notifications_supervisor_assigned |
