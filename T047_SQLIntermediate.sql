USE DotNetCourseDatabase
GO

SELECT [Users].[UserId],
        [Users].[FirstName] + ' ' + [Users].[LastName] AS FullName,
        [Users].[Email],
        [Users].[Gender],
        [Users].[Active],
        [UserJobInfo].[JobTitle],
        [UserJobInfo].[Department],
        [UserSalary].[Salary]
        FROM TutorialAppSchema.Users AS Users
                JOIN TutorialAppSchema.UserSalary AS UserSalary
                        ON Users.UserId = UserSalary.UserId
                LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
                        ON Users.UserId = UserJobInfo.UserId
        WHERE Users.Active = 1
        ORDER BY Users.UserId DESC


DELETE FROM TutorialAppSchema.UserJobInfo WHERE UserId > 500

DELETE FROM TutorialAppSchema.UserSalary WHERE UserId BETWEEN 250 AND 750


SELECT
        [UserSalary].[UserId],
        [UserSalary].[Salary]
        FROM TutorialAppSchema.UserSalary AS UserSalary
        WHERE EXISTS (
                SELECT * FROM TutorialAppSchema.UserJobInfo AS UserJobInfo
                        WHERE UserJobInfo.UserId = UserSalary.UserId)
                AND UserId <> 7


SELECT
        [UserId],
        [Salary]
        FROM TutorialAppSchema.UserSalary
--UNION
UNION ALL
SELECT
        [UserId],
        [Salary]
        FROM TutorialAppSchema.UserSalary


SELECT [Users].[UserId],
        [Users].[FirstName] + ' ' + [Users].[LastName] AS FullName,
        [Users].[Email],
        [Users].[Gender],
        [Users].[Active],
        [UserJobInfo].[JobTitle],
        [UserJobInfo].[Department],
        [UserSalary].[Salary]
        FROM TutorialAppSchema.Users AS Users
                JOIN TutorialAppSchema.UserSalary AS UserSalary
                        ON Users.UserId = UserSalary.UserId
                LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
                        ON Users.UserId = UserJobInfo.UserId
        WHERE Users.Active = 1
        ORDER BY Users.UserId DESC


CREATE CLUSTERED INDEX cix_UserSalary_UserId ON TutorialAppSchema.UserSalary(UserId)


CREATE NONCLUSTERED INDEX ix_UserJobInfo_JobTitle ON TutorialAppSchema.UserJobInfo(JobTitle)
        INCLUDE (Department)


CREATE NONCLUSTERED INDEX fix_Users_Active ON TutorialAppSchema.Users(Active)
        INCLUDE ([Email], [FirstName], [LastName])
                WHERE Active = 1


SELECT
        ISNULL([UserJobInfo].[Department], 'No Department Listed') AS Department,
        SUM([UserSalary].[Salary]) AS Salary,
        MIN([UserSalary].[Salary]) AS MinSalary,
        MAX([UserSalary].[Salary]) AS MaxSalary,
        AVG([UserSalary].[Salary]) AS AvgSalary,
        COUNT(*) AS PeopleInDepartment,
        STRING_AGG(Users.UserId, ', ') AS UserIDs
        FROM TutorialAppSchema.Users AS Users
                JOIN TutorialAppSchema.UserSalary AS UserSalary
                        ON Users.UserId = UserSalary.UserId
                LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
                        ON Users.UserId = UserJobInfo.UserId
        WHERE Users.Active = 1
        GROUP BY [UserJobInfo].[Department]
        ORDER BY [UserJobInfo].[Department] DESC


SELECT [Users].[UserId],
        [Users].[FirstName] + ' ' + [Users].[LastName] AS FullName,
        [UserJobInfo].[JobTitle],
        [UserJobInfo].[Department],
        DepartmentAverage.AvgSalary,
        [UserSalary].[Salary],
        [Users].[Email],
        [Users].[Gender],
        [Users].[Active]
        FROM TutorialAppSchema.Users AS Users
                JOIN TutorialAppSchema.UserSalary AS UserSalary
                        ON Users.UserId = UserSalary.UserId
                LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
                        ON Users.UserId = UserJobInfo.UserId
                OUTER APPLY(
                        SELECT ISNULL([UserJobInfo2].[Department], 'No Department Listed') AS Department,
                                AVG([UserSalary2].[Salary]) AS AvgSalary
                                FROM TutorialAppSchema.UserSalary AS UserSalary2
                                        LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo2
                                                ON UserSalary2.UserId = UserJobInfo2.UserId
                                WHERE ISNULL([UserJobInfo2].[Department], 'No Department Listed') = ISNULL([UserJobInfo].[Department], 'No Department Listed')
                                GROUP BY [UserJobInfo2].[Department]
                ) AS DepartmentAverage
        WHERE Users.Active = 1
        ORDER BY Users.UserId DESC


SELECT GETDATE()

SELECT DATEADD(YEAR, -5, GETDATE())

SELECT DATEDIFF(MINUTE, GETDATE(), DATEADD(YEAR, -5, GETDATE()))


ALTER TABLE TutorialAppSchema.UserSalary ADD AvgSalary DECIMAL(18,4)

SELECT * FROM TutorialAppSchema.UserSalary
