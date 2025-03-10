// fake_post.js
import { faker } from '@faker-js/faker';
import mysql from 'mysql2';

// Create a connection to the database
const connection = mysql.createConnection({
  host: 'localhost',  // or your MySQL host
  user: 'root',       // MySQL username
  password: '77777778910a',  // MySQL password
  database: 'purple_media',  // Database name
});

connection.connect((err) => {
  if (err) {
    console.error('Error connecting to the database:', err);
    return;
  }
  console.log('Connected to the database');
});

function formatDateForMySQL(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');
  
    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
  }
function generateFakePost(userId) {
  return {
    postId: 0, // You can auto-increment this in MySQL or set it manually
    title: faker.lorem.sentence(),
    content: faker.lorem.paragraphs(),
    createdAt: formatDateForMySQL(faker.date.past()), // Format the date to MySQL-compatible format
    userId: userId, // Randomly assigned userId
  };
}

function insertFakePost(post) {
  const postQuery = 'INSERT INTO Posts (postId, title, content, createdAt, userId) VALUES (?, ?, ?, ?, ?)';
  connection.query(postQuery, [post.postId, post.title, post.content, post.createdAt, post.userId], (err, postResults) => {
    if (err) {
      console.error('Error inserting post:', err);
      return;
    }
    console.log('Inserted post:', postResults);
  });
}

// Generate a random number of posts (adjust as needed)
for (let i = 0; i < 200; i++) {
  const randomUserId = Math.floor(Math.random() * 10); // Randomly choose a userId between 0 and 9
  const post = generateFakePost(randomUserId);
  insertFakePost(post);
}

connection.end();
