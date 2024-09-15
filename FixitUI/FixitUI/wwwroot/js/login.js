document.addEventListener('DOMContentLoaded', () => {
   
    const loginButton = document.querySelector('button[type="button"]');
    if (loginButton) {
   
        loginButton.addEventListener('click', async () => {

            const username = document.getElementById('username').value.trim();
            const password = document.getElementById('password').value;
            if (username === '' || password === '') {
                alert('Please enter both username and password.');
                return;
            }
            const requestBody = JSON.stringify({ username, password });
            try {
                const response = await fetch('https://localhost:7105/api/Account/login', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: requestBody
                });
                if (response.ok) {
                    const result = await response.json();
                    const token = result.token;
                    localStorage.setItem('jwtToken', token);
                    window.location.href = '/dashboard';
                } else {
                    alert('Invalid username or password.');
                }
            }
            catch (error) {
                console.error('Error:', error);
                alert('An error occurred while trying to log in. Please try again.');
            }
        });
    }
});
