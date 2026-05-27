--------------->>SECTION 1
---1. To find the top 3 highest paid employees based on their average net salary.
SELECT TOP 3
	e.name
	, e.department
	, sum(p.net_salary) AS total_net_salary
FROM Employees e
INNER JOIN Payroll p
	ON e.employee_id = p.employee_id
GROUP BY 
	e.employee_id
	, e.name 
	, e.department
ORDER BY AVG(p.net_salary) DESC

--2. To calculate the total gross salary, total tax deducted, and average net salary for each department.
SELECT
	e.department
	, sum(p.net_salary) AS 'total gross salary'
	, sum(p.tax_amount) AS 'total tax deducted'
	, avg(p.net_salary) AS 'average net salary'
FROM Employees e
INNER JOIN Payroll p
	ON e.employee_id = p.employee_id
GROUP BY 
	e.department


--------------->>SECTION 2
--3. To find the employees who have not received any payment in the year 2024.
SELECT *
FROM Employees e
INNER JOIN Payroll p
	ON e.employee_id = p.employee_id
WHERE YEAR(pay_date) <> 2024

--4. To find the most recent payment date and net salary for each employee.
SELECT 
	sub.employee_id
	,sub.name
	,sub.pay_date
	,sub.net_salary
FROM  (
	SELECT 
		e.employee_id
		,e.name
		,p.pay_date
		,p.net_salary
		,ROW_NUMBER() OVER (PARTITION BY e.employee_id ORDER BY p.pay_date DESC) AS RN
	FROM Employees e
	INNER JOIN Payroll p
		ON e.employee_id = p.employee_id
) AS sub
WHERE sub.RN = 1

--------------->>SECTION 3

--5. Apply indexing to pay_date and employee_id so it can quickly find the date specified in the WHERE clause and join on employee_id.
--6. It is innefficient because it is selecting all columns with SELECT *, which can lead to unnecessary data retrieval. Instead, it should specify only the necessary columns to improve performance and reduce memory usage.

--------------->>SECTION 4
--7. To rank employees within their respective departments based on their total net salary.
SELECT
  e.employee_id
  , e.name
  , e.department
  , p.total_net
  , RANK() OVER (PARTITION BY e.department ORDER BY p.total_net DESC) AS dept_rank
FROM (
  SELECT 
	employee_id
	, SUM(net_salary) AS total_net
  FROM Payroll
  GROUP BY employee_id
) p
JOIN Employees e 
	ON e.employee_id = p.employee_id
ORDER BY 
	e.department
	, dept_rank;
