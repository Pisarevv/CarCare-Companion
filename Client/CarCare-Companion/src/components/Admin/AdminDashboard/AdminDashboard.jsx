import { Link } from 'react-router-dom';
import './AdminDashboard.css'

const AdminDashboard = () => {
    
    return(
        <section className="admin-home">
            <div className="admin-home-container">
                <div className='admin-welcoming'>
                    <div>Welcome to the admin dashboard.</div>
                    <div>Here you can view all the users,add and remove admins, edit carousel ads.</div>
                </div>

                <div className='admin-controls'>
                <div><Link to = "/Administrator/ApplicationUsers">Application users</Link></div>
                <div>Edit carousel ads</div>
                </div>
            </div>
        </section>
    )
}

export default AdminDashboard;