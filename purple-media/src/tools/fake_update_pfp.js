import { faker } from '@faker-js/faker';
import fetch from 'node-fetch';

// Function to introduce a delay
function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function updateFakePfp() {
    try {
        // Generate a random user ID and profile picture URL
        const userId = faker.number.int({ min: 1, max: 100 });
        const pfpUrl = 'person_' + faker.number.int({ min: 1, max: 100 }) + '.jpg';

        // Construct the URL with the query parameter
        const url = `http://localhost:5101/api/User/${userId}/ProfilePicture?profilePicturePath=${encodeURIComponent(pfpUrl)}`;

        // Send the update request
        const updateResponse = await fetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json', // Optional, since no body is sent
            },
        });

        // Handle the response
        if (updateResponse.ok) {
            console.log(`Profile picture successfully updated for user: ${userId}`);
        } else {
            const errorResponse = await updateResponse.json();
            console.error(`Failed to update profile picture for user ${userId}:`, errorResponse);
        }
    } catch (error) {
        console.error('Error updating profile picture:', error);
    }
}

// Function to run updates with a delay
async function runUpdates() {
    for (let i = 0; i < 1000; i++) {
        await updateFakePfp();
    }
}

// Run the script
runUpdates();