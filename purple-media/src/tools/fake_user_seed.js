import mysql from 'mysql2';
import { faker } from '@faker-js/faker';

function formatDateForMySQL(date) {
    const d = new Date(date);
    return d.toISOString().slice(0, 19).replace('T', ' '); // Converts to 'YYYY-MM-DD HH:MM:SS'
}

// Set up MySQL connection
const connection = mysql.createConnection({
    host: 'localhost', // Replace with your DB host
    user: 'root', // Replace with your DB username
    password: '77777778910a', // Replace with your DB password
    database: 'purple_media', // Replace with your database name
});

connection.connect((err) => {
    if (err) {
        console.error('Error connecting to MySQL:', err.stack);
        return;
    }
    console.log('Connected to MySQL as id ' + connection.threadId);
});

function generateFakeUser() {
    return {
        userId: 0, // You can auto-increment this in MySQL or set it manually
        email: faker.internet.email(),
        username: faker.internet.userName(),
        passwordHash: faker.internet.password(), // Ensure to hash the password before storing it in production
        createdAt: formatDateForMySQL(faker.date.past()),
    };
}

function insertFakeUsers(numberOfUsers) {
    for (let i = 0; i < numberOfUsers; i++) {
        const user = generateFakeUser();
        const query = 'INSERT INTO Users (email, username, passwordHash, createdAt) VALUES (?, ?, ?, ?)';
        connection.query(query, [user.email, user.username, user.passwordHash, user.createdAt], (err, result) => {
            if (err) {
                console.error('Error inserting user:', err);
                return;
            }
            console.log('Inserted user with id:', result.insertId);
        });
    }
}

// Insert 100 fake users
insertFakeUsers(100);

// Close the connection
connection.end();
