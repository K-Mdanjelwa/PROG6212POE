SELECT 
    L.LecturerId,
    L.LName + ' ' + L.LSName AS LecturerName,
    L.HoursWorked,
    L.HourRate,
    (L.HoursWorked * L.HourRate) AS Amount,
    T.TStatus
FROM Lecturer L
JOIN Track T ON L.LecturerId = T.LecturerId
WHERE T.TrackId = @TrackID;