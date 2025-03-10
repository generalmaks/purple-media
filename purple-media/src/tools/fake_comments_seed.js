// fake_post.js
import { faker } from '@faker-js/faker';

async function insertFakeComment() {
  let postId = faker.number.int({ min: 1, max: 175 });
  let content = faker.lorem.paragraph();
  let authorId = faker.number.int({ min: 1, max: 100 });
  
  
  const fakeComment = {
    postId,
    content,
    authorId
  };

  try{
    const response = await fetch('http://localhost:5101/api/Comment', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(fakeComment),
    });
    const data = await response.json();
    console.log(data);

    if (response.ok) {
      console.log('Comment successfully posted:', fakeComment);
    } else {
      console.error('Failed to post comment:', response.statusText);
    }
  } catch (error) {
    console.error('Error:', error);
  }
}


// Generate a random number of posts (adjust as needed)
for (let i = 0; i < 1000; i++) {
  insertFakeComment()
}
