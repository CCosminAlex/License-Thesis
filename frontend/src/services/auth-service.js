import axios from 'axios';

const API_URL = 'https://localhost:44333/api';

class AuthService {
  login(user) {
    return axios
      .post(API_URL + '/Authenticate/login', {
        email: user.email,
        password: user.password
      })
      .then(response => {
        if (response.data.token) {
          localStorage.setItem('user', JSON.stringify(response.data));
        }
        
        return response.data;
      });
  }

  logout() {
    localStorage.clear();
  }

  register(user) {
    return axios.post(API_URL + '/Authenticate/register', {
      firstName: user.firstName,
      email: user.email,
      password: user.password,
      lastName: user.lastName,
      middleName:user.middleName,
      phone:user.phone,

    });
  }
}

export default new AuthService();