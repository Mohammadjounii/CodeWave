# Force Seed Job Offers

If job offers are not appearing in the database, try one of these solutions:

## Solution 1: Restart the Application
The seed runs automatically on application startup. Simply restart your application and check the console for seed messages.

## Solution 2: Clear Existing Jobs and Re-seed
If you want to clear all jobs and re-seed:

```sql
-- WARNING: This deletes ALL job offers including applications
DELETE FROM JobApplications;
DELETE FROM JobOffers;
```

Then restart the application.

## Solution 3: Check Console Output
When you start the application, look for these messages:
- "Seeding job offers. X existing jobs found."
- "✅ Successfully seeded X job offers."
- "ℹ️ All job offers already exist. No new jobs added."

If you see an error message, that's the issue.

## Solution 4: Manual Seed via Database
If the seed isn't working, the jobs will be automatically added when you restart the application. The seed method now:
- Checks which jobs already exist
- Only adds missing jobs
- Won't create duplicates

## Verify Jobs in Database
Run this SQL query to check:
```sql
SELECT COUNT(*) as JobCount FROM JobOffers WHERE isDeleted = 0;
```

You should see 19 job offers after seeding.

