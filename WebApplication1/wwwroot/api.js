const url = "http://localhost:5053/api/patient";

const params = new URLSearchParams(location.search);
const p_id = params.get("id");



function get_patient_Data(p_id = null) {
    const data = {
        patient_id: p_id,
        sorttype: "patient_id",
        page_no: 1,
        page_size: 5
    };

    if (p_id) {
        fetch(url + '/patientget', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        }).then((Response) => {
            return Response.json();
        }).then((data) => {
            document.getElementById('fname').value = data[0].p_firstname;
            document.getElementById('mname').value = data[0].p_middlename;
            document.getElementById('lname').value = data[0].p_lastname;
            document.getElementById('p_sex').value = data[0].p_sex_type;
            document.getElementById('dob').value = data[0].patient_dob.substr(0, 10);
            document.getElementById('chart_no').innerHTML = data[0].chart_no;
        })
    }
}

document.addEventListener('DOMContentLoaded', get_patient_Data(p_id));


function postputdata(e) {
    e.preventDefault();
    if (p_id) {
        e.preventDefault();
        const data = {
            patient_id: p_id,
            firstname: document.getElementById('fname').value,
            lastname: document.getElementById('lname').value,
            middlename: document.getElementById('mname').value,
            sex_type: document.getElementById('p_sex').value,
            dob: document.getElementById('dob').value.substr(0,10)
        }
        console.log(document.getElementById('dob').value.substr(0, 10));
        fetch(url + '/patientupdate', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        }).then((Response) => {
            return Response.json();
        }).then( ()=> {
            alert("patient data updated successfully");
        }).catch((erroe) => {
            alert("patient data is not updated ");
        });
    }
    else {
        e.preventDefault();
        const data_post = {
            firstname: document.getElementById('fname').value,
            lastname: document.getElementById('lname').value,
            middlename: document.getElementById('mname').value,
            sex_type: document.getElementById('p_sex').value,
            dob: new Date(document.getElementById('dob').value.substr(0, 10))
        }
        
        fetch(url + '/patientinsert',
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data_post)
            }).then((Response) => {
                return Response.json();
            }).then((data) => {
                alert("patient inserted succesfully");
                window.location.href = `http://localhost:5053/?id=${data[0].patientcreate}`;
            }).catch(error => {
                alert("Unable to save patient data.");
                console.error('Unable to save patient data.', error);
            });
    }
}

function resetdata(e) {
    e.preventDefault();
    document.getElementById("pt-data").reset();

    const param = new URLSearchParams(location.search);
    const pid = param.get("id");
    /*    console.log(pid);*/
    get_patient_Data(pid);

}

function _toastr() {
    let toastr = document.getElementById("toastr");
    let title = document.getElementById("toastr-title");
    let text = document.getElementById("toastr-text");

    toastr.style.display = "inline";
    toastr.style.zIndex = 99;
    toastr.style.opacity = 0.80;
    status === "Sucess" ? toastr.style.background = "green" : toastr.style.background = "red";
    status === "Sucess" ? title.innerHTML = "Success" : title.innerHTML = "Error";


}

//fetch method to insert call post method for insert patient data
function postData(e) {
    e.preventDefault();
    const data_post = {
        firstname: document.getElementById('fname').value,
        lastname: document.getElementById('lname').value,
        middlename: document.getElementById('mname').value,
        sex_type: document.getElementById('p_sex').value,
        dob: document.getElementById('dob').value
    }
    fetch(url + '/patientinsert',
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data_post)
        }).then((Response) => {
            return Response.json();
        }).then((data) => {
            get_patient_Data(data[0].patientcreate);
        }).catch(error => console.error('Unable to save patient data.', error));
}

function putData() {
    const data = {
        patient_id: p_id,
        firstname: document.getElementById('fname').value,
        lastname: document.getElementById('lname').value,
        middlename: document.getElementById('mname').value,
        sex_type: document.getElementById('p_sex').value,
        dob: document.getElementById('dob').value
    }
    fetch(url + '/patientupdate', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }).then((Response) => {
        return Response.json();
    })
}

