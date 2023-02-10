function reset_form() {
    document.getElementById("pt-data").reset();
}

/*
function sub_1() {
    let info = document.getElementById("dob").value;
    let info_sex = document.getElementById("p_sex").value;
    var dob = new Date(info);

    var month_diff = Date.now() - dob.getTime();
    var age_dt = new Date(month_diff);
    var year = age_dt.getUTCFullYear();
    //var year_u = info.dob.getUTCFullYear();
    var age = Math.abs(year - 1970);
    if (age < 18) {
        var gender = "HE/SHE";
        if (info_sex == "male") {
            gender = "He";
        } else if (info_sex == 'she') {
            gender = "She";
        }
        alert("Please add a contact for the Patient as " + gender + " is a Minor.");
    }

    const form = document.getElementById("pt-data");
    const formData = new FormData(form);
    let data = {};
    for (item of formData) {
        data[item[0]] = item[1];
        // console.log(item[0] +"     =" +item[1]);
    }
    console.log(data);
};*/

function nav_collaps() {
    let l = document.getElementById("left");
    let m = document.getElementById("mid");
    document.getElementById("collaps").style.display = "none";
    document.getElementById("expand").style.display = "inline";
    document.getElementById("collaps").style.marginLeft = "5px";
    l.style.flex = "2%";
    m.style.flex = "95%";

};
function nav_expand() {
    let l = document.getElementById("left");
    let m = document.getElementById("mid");
    document.getElementById("collaps").style.display = "inline";
    document.getElementById("expand").style.display = "none";
    l.style.flex = "12%";
    m.style.flex = "85%";
};
function add_home() {
    document.querySelector('.cont-home-work').insertAdjacentHTML("beforeend", `<fieldset class="home-cd border" id="cont1">
                                <legend class="border legend-con-det" style=" border-radius: 5px; padding: 0.2em 0.8em ">
                                    <div class="home">
                                        <select name="homel" id="homel">
                                            <option value="home">Home</option>
                                            <option value="work">Work</option>
                                            <option value="other">Other</option>
                                        </select><br>
                                    </div>
                                </legend>
                                <div class="home-work">
                                    <div class="address-parent">
                                        <div class="add-plus">
                                            <a href="#" style=" text-decoration: none;" class="plus">
                                                <h4 class="hed-cont">Address</h4>
                                                <i id="plus-add" style="display: none;" class="fa fa-plus-circle" onclick="add_address(this)"></i>
                                            </a>
                                            <a href="#" style="text-decoration: none;" class="trash"
                                               onclick="delete_home(this)">
                                                <i class="fa fa-trash" aria-hidden="true"></i>
                                            </a>
                                        </div>
                                        <div class="address" id="address">
                                            <div class="street">
                                                <label for="street">Street:</label><br>
                                                <input type="text" id="street" name="street">
                                            </div>
                                            <div class="add">
                                                <div class="add-info">
                                                    <label for="zip">Zip:</label><br>
                                                    <input type="text" id="zip" name="zip">
                                                </div>
                                                <div class="add-info">
                                                    <label for="city">City:</label><br>
                                                    <input type="text" id="city" name="city">
                                                </div>
                                                <div class="add-info">
                                                    <label for="state">State:</label><br>
                                                    <input type="text" id="state" name="state">
                                                </div>
                                                <div class="add-info">
                                                    <label for="country">Country:</label><br>
                                                    <input type="text" id="country" name="country">
                                                </div>
                                                <div class="add-info">
                                                    <a href="#" style="text-decoration: none;" class="trash"
                                                       id="trash" onclick="delete_address(this)">
                                                        <i class="fa fa-trash" aria-hidden="true"></i>
                                                    </a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="phone-parent mrgn-tp">
                                        <h4 class="hed-cont">Phone</h4>
                                        <a href="#" style="text-decoration: none;" class="plus">
                                            <i class="fa fa-plus-circle" onclick="add_phone_det(this)"></i>
                                        </a>
                                        <div class="phone-det" id="phone-det1">
                                            <div class="phone-info">
                                                <label for="type">Type</label><br>
                                                <hr class="solid">
                                                <select name="type" id="type">
                                                    <option value="none" selected disabled hidden></option>
                                                    <option value="cell">Cell</option>
                                                    <option value="landline">Landline</option>
                                                </select>
                                            </div>
                                            <div class="phone-info">
                                                <label for="code">Code</label><br>
                                                <hr class="solid">
                                                <select name="code" id="code">
                                                    <option value="none" selected disabled hidden></option>
                                                    <option value="+1">+1 UnitedState</option>
                                                    <option value="+93">+93 Afghanistan</option>
                                                    <option value="+91">+91 India</option>
                                                </select>
                                            </div>
                                            <div class="phone-info">
                                                <label for="number">Number</label><br>
                                                <hr class="solid">
                                                <input type="text" id="number" name="number">
                                            </div>
                                            <div class="phone-info">
                                                <label for="ext">Ext</label><br>
                                                <hr class="solid">
                                                <a href="#" style="text-decoration: none;" class="trash"
                                                   onclick="delete_phone_det(this)">
                                                    <i class="fa fa-trash" aria-hidden="true"></i>
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="fax-parent mrgn-tp">
                                        <h4 class="hed-cont">Fax</h4>
                                        <a href="#" style="text-decoration: none;" class="plus">
                                            <i class="fa fa-plus-circle" onclick="add_fax(this)"></i>
                                        </a>
                                    </div>
                                    <div class="email-parent mrgn-tp">
                                        <h4 class="hed-cont">Email</h4>
                                        <a href="#" style="text-decoration: none;" class="plus">
                                            <i class="fa fa-plus-circle" onclick="add_email(this)"></i>
                                        </a>
                                        <div class="email" id="email1">
                                            <div class="email-dt">
                                                <input type="email" id="email" name="email">
                                            </div>
                                            <div class="email-dt">
                                                <a href="#" style="text-decoration: none;" class="trash"
                                                   onclick="delete_email(this)">
                                                    <i class="fa fa-trash" aria-hidden="true"></i>
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="web mrgn-tp">
                                        <h4 class="hed-cont">Website</h4>
                                        <a href="#" style="text-decoration: none;" id="plus" class="plus">
                                            <i class="fa fa-plus-circle" onclick="add_website(this)" id="web+"></i>
                                        </a>
                                    </div>
                                </div>
                            </fieldset>`);
}
function delete_home(inp) {
    inp.parentNode.parentNode.parentNode.parentNode.remove();
};

function add_address(i) {
    i.parentNode.parentNode.insertAdjacentHTML("afterend", `<div class="address" id="address">
                                            <div class="street">
                                                <label for="street">Street:</label><br>
                                                <input type="text" id="street" name="street">
                                            </div>
                                            <div class="add">
                                                <div class="add-info">
                                                    <label for="zip">Zip:</label><br>
                                                    <input type="text" id="zip" name="zip">
                                                </div>
                                                <div class="add-info">
                                                    <label for="city">City:</label><br>
                                                    <input type="text" id="city" name="city">
                                                </div>
                                                <div class="add-info">
                                                    <label for="state">State:</label><br>
                                                    <input type="text" id="state" name="state">
                                                </div>
                                                <div class="add-info">
                                                    <label for="country">Country:</label><br>
                                                    <input type="text" id="country" name="country">
                                                </div>
                                                <div class="add-info">
                                                    <a href="#" style="text-decoration: none;" class="trash"
                                                       id="trash" onclick="delete_address(this)">
                                                        <i class="fa fa-trash" aria-hidden="true"></i>
                                                    </a>
                                                </div>
                                            </div>
                                        </div>`);
    i.style.display = "none";
}
function delete_address(i) {
    let v = i.parentNode.parentNode.parentNode;
    v.previousElementSibling.innerHTML = `<a href="#" style=" text-decoration: none;" class="plus">
                                                <h4 class="hed-cont">Address</h4>
                                                <i id="plus-add"  class="fa fa-plus-circle" onclick="add_address(this)"></i>
                                            </a>
                                            <a href="#" style="text-decoration: none;" class="trash"
                                               onclick="delete_home(this)">
                                                <i class="fa fa-trash" aria-hidden="true"></i>
                                            </a>`
    v.remove();
};

function add_phone_det(i) {
    i.parentNode.parentNode.insertAdjacentHTML("afterend", `<div class="phone-det" id="phone-det1">
                                            <div class="phone-info">
                                                <label for="type">Type</label><br>
                                                <hr class="solid">
                                                <select name="type" id="type">
                                                    <option value="none" selected disabled hidden></option>
                                                    <option value="cell">Cell</option>
                                                    <option value="landline">Landline</option>
                                                </select>
                                            </div>
                                            <div class="phone-info">
                                                <label for="code">Code</label><br>
                                                <hr class="solid">
                                                <select name="code" id="code">
                                                    <option value="none" selected disabled hidden></option>
                                                    <option value="+1">+1 UnitedState</option>
                                                    <option value="+93">+93 Afghanistan</option>
                                                    <option value="+91">+91 India</option>
                                                </select>
                                            </div>
                                            <div class="phone-info">
                                                <label for="number">Number</label><br>
                                                <hr class="solid">
                                                <input type="text" id="number" name="number">
                                            </div>
                                            <div class="phone-info">
                                                <label for="ext">Ext</label><br>
                                                <hr class="solid">
                                                <a href="#" style="text-decoration: none;" class="trash"
                                                   onclick="delete_phone_det(this)">
                                                    <i class="fa fa-trash" aria-hidden="true"></i>
                                                </a>
                                            </div>
                                        </div>`);
};
function delete_phone_det(inp) {
    inp.parentNode.parentNode.remove();
};

function add_fax(i) {
    i.parentNode.parentNode.insertAdjacentHTML("afterend", `<div class="fax" id="fax1">
    <div class="fax-dt">
        <label for="code">Code</label><br>
        <hr class="solid">
        <select name="code" id="code">
            <option value="none" selected disabled hidden></option>
            <option value="+1">+1</option>
            <option value="+93">+93</option>
            <option value="+91">+91</option>
        </select>
    </div>
    <div class="fax-dt">
        <label for="number">Number</label><br>
        <hr class="solid">
        <input type="text" id="number" name="number">
    </div>
    <div class="fax-dt">
        <label for="null"></label><br>
        <hr class="solid">
        <a href="#" style="text-decoration: none;" class="trash"
            onclick="delete_fax(this)">
            <i class="fa fa-trash" aria-hidden="true"></i></a>
    </div>
    <div class="fax-dt">
        <label for="null"></label><br>
        <hr class="solid">
    </div>
</div>`);
};
function delete_fax(inp) {
    inp.parentNode.parentNode.remove();
};


function add_email(i) {
    i.parentNode.parentNode.insertAdjacentHTML("afterend", `<div class="email" id="email1">
    <div class="email-dt">
        <input type="email" id="email" name="email">
    </div>
    <div class="email-dt">
        <a href="#" style="text-decoration: none;" class="trash"
            onclick="delete_email(this)">
            <i class="fa fa-trash" aria-hidden="true"></i></a>
    </div>
</div>`);
};
function delete_email(inp) {
    inp.parentNode.parentNode.remove();
};


function add_website(i) {
    i.parentNode.parentNode.insertAdjacentHTML("afterend", `<div class="website" id="website1">
    <div class="website-dt">
        <input type="text" id="website" name="website">
    </div>
    <div class="website-dt">
        <a href="#" style="text-decoration: none;" class="trash"
            onclick="delete_website(this)">
            <i class="fa fa-trash" aria-hidden="true"></i></a>
    </div>
</div>`);
};
function delete_website(inp) {
    inp.parentNode.parentNode.remove();
};

function otherdetail() {
    let x = document.getElementById("oth-det");
    if (x.style.display === "flex") {
        x.style.display = "none";
        document.getElementById("c_right").style.display = "inline";
        document.getElementById("c_down").style.display = "none";
    } else {
        x.style.display = "flex";
        document.getElementById("c_right").style.display = "none";
        document.getElementById("c_down").style.display = "inline";
    }
};
