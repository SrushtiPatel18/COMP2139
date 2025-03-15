CREATE TABLE Projects (
    ProjectId SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    StartDate TIMESTAMP,
    EndDate TIMESTAMP
);
CREATE TABLE ProjectTasks (
    TaskId SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    ProjectId INT NOT NULL,
    FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId) ON DELETE CASCADE
);
