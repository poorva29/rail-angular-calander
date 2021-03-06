class PostsController < ApplicationController
  before_action :set_post, only: [:show, :edit, :update, :destroy]

  # GET /posts
  # GET /posts.json
  def index
    @posts = Post.all
  end

  # GET /posts/1
  # GET /posts/1.json
  def show
  end

  # GET /posts/new
  def new
    @post = Post.new
  end

  # GET /posts/1/edit
  def edit
  end

  # POST /posts
  # POST /posts.json
  def create
    @post = Post.new(post_params)

    respond_to do |format|
      if @post.save
        format.html { redirect_to @post, notice: 'Post was successfully created.' }
        format.json { render action: 'show', status: :created, location: @post }
      else
        format.html { render action: 'new' }
        format.json { render json: @post.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /posts/1
  # PATCH/PUT /posts/1.json
  def update
    respond_to do |format|
      if @post.update(post_params)
        format.html { redirect_to @post, notice: 'Post was successfully updated.' }
        format.json { head :no_content }
      else
        format.html { render action: 'edit' }
        format.json { render json: @post.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /posts/1
  # DELETE /posts/1.json
  def destroy
    @post.destroy
    respond_to do |format|
      format.html { redirect_to posts_url }
      format.json { head :no_content }
    end
  end

  def doctor_locations
    docloc_json = [
      {
        id: '1',
        firstname: 'Poorva',
        lastname: 'Mahajan',
        locations: [
          { id: 3, name: 'Akurdi' },
          { id: 4, name: 'Kalyani Nagar' }
        ]
      },
      {
        id: '2',
        firstname: 'Rutuja',
        lastname: 'Khanpekar',
        locations: [
          { id: 5, name: 'Swargate' },
          { id: 6, name: 'Dahanukar' }
        ]
      }
    ]
    render json: docloc_json
  end

  def events
    if params[:location].eql?('3') || params[:location].eql?('6')
      events_json = {
        calendar: {
          slot_duration: '00:15:00',
          min: '1030',
          max: '2000'
        },
        events: [
          {
            id: 1,
            start: '2015-08-01T15:00',
            end: '2015-08-01T18:00',
            event_type: 'blocked',
            appointment_type: 'OPD',
            patient_name: '',
            subject: 'foo'
          },
          {
            id: 2,
            start:  '2015-08-04T11:00',
            end: '2015-08-04T13:00',
            event_type: 'booking',
            patient_name: 'Poorva Mahajan',
            appointment_type: 'Patient Appointment',
            subject: 'boo',
            prepay_amount: 10,
            is_paid: true
          },
          {
            id: 3,
            start:  '2015-07-30T11:00',
            end: '2015-07-30T12:00',
            event_type: 'booking',
            patient_name: 'Test User',
            appointment_type: '',
            subject: 'boo',
            prepay_amount: 20,
            is_paid: false
          },
          {
            id: -1,
            start: '2015-07-31T17:00',
            end: '2015-07-31T19:00',
            event_type: 'non-working',
            patient_name: '',
            appointment_type: ''
          },
          {
            id: -1,
            start: '2015-08-01T12:00',
            end: '2015-08-01T14:00',
            event_type: 'non-working',
            patient_name: '',
            appointment_type: ''
          }
        ],
        work_hrs: [
          {
            friday: true,
            monday: true,
            saturday: false,
            sunday: false,
            thursday: true,
            tuesday: true,
            wednesday: true,
            created_by: 18467,
            created_on: "2015-08-07T11:34",
            doclocationid: 3,
            doclocworkinghoursid: 6349,
            fromtime: "0730",
            modified_by: 18467,
            modified_on: "2015-08-07T11:34",
            totime: "1200"
          },
          {
            friday: true,
            monday: true,
            saturday: false,
            sunday: false,
            thursday: true,
            tuesday: false,
            wednesday: true,
            created_by: 18467,
            created_on: "2015-08-07T11:36",
            doclocationid: 4,
            doclocworkinghoursid: 6350,
            fromtime: "1245",
            modified_by: 18467,
            modified_on: "2015-08-07T11:36",
            totime: "1415"
          },
          {
            friday: false,
            monday: false,
            saturday: false,
            sunday: false,
            thursday: false,
            tuesday: true,
            wednesday: false,
            created_by: 18467,
            created_on: "2015-08-07T11:38",
            doclocationid: 13981,
            doclocworkinghoursid: 6351,
            fromtime: "1245",
            modified_by: 18467,
            modified_on: "2015-08-07T11:38",
            totime: "1415"
          }
        ]
      }
    else
      events_json = {
        calendar: {
          slot_duration: '00:15:00',
          min: '900',
          max: '1830'
        },
        events: [
          {
            id: 1,
            start:  '2015-07-26T15:00',
            end: '2015-07-26T17:00',
            event_type: 'blocked',
            appointment_type: 'OPD',
            patient_name: '',
            subject: 'foo'
          },
          {
            id: 2,
            start:  '2015-08-07T13:00',
            end: '2015-08-0715:00',
            event_type: 'booking',
            appointment_type: 'Patient Appointment',
            patient_name: 'Rutuja Khanpekar',
            subject: 'boo',
            prepay_amount: 0,
            is_paid: false
          },
          {
            id: 3,
            start:  '2015-07-24T10:00',
            end: '2015-07-24T11:00',
            event_type: 'booking',
            appointment_type: '',
            patient_name: 'Test User',
            subject: 'boo',
            prepay_amount: 10,
            is_paid: false
          },
          {
            id: -1,
            start: '2015-07-23T10:00',
            end: '2015-07-23T12:00',
            event_type: 'non-working',
            patient_name: '',
            appointment_type: ''
          },
          {
            id: -1,
            start: '2015-07-23T13:00',
            end: '2015-07-23T15:00',
            event_type: 'non-working',
            patient_name: '',
            appointment_type: ''
          }
        ],
        work_hrs: [
          {
            friday: false,
            monday: false,
            saturday: false,
            sunday: false,
            thursday: true,
            tuesday: true,
            wednesday: true,
            created_by: 18467,
            created_on: "2015-08-07T11:34",
            doclocationid: 3,
            doclocworkinghoursid: 6349,
            fromtime: "0730",
            modified_by: 18467,
            modified_on: "2015-08-07T11:34",
            totime: "1200"
          },
          {
            friday: false,
            monday: true,
            saturday: false,
            sunday: false,
            thursday: true,
            tuesday: true,
            wednesday: true,
            created_by: 18467,
            created_on: "2015-08-07T11:34",
            doclocationid: 4,
            doclocworkinghoursid: 6349,
            fromtime: "0730",
            modified_by: 18467,
            modified_on: "2015-08-07T11:34",
            totime: "1200"
          },
          {
            friday: true,
            monday: false,
            saturday: false,
            sunday: false,
            thursday: true,
            tuesday: true,
            wednesday: true,
            created_by: 18467,
            created_on: "2015-08-07T11:34",
            doclocationid: 5,
            doclocworkinghoursid: 6349,
            fromtime: "0730",
            modified_by: 18467,
            modified_on: "2015-08-07T11:34",
            totime: "1200"
          },
          {
            friday: false,
            monday: false,
            saturday: false,
            sunday: false,
            thursday: true,
            tuesday: false,
            wednesday: true,
            created_by: 18467,
            created_on: "2015-08-07T11:36",
            doclocationid: 6,
            doclocworkinghoursid: 6350,
            fromtime: "1245",
            modified_by: 18467,
            modified_on: "2015-08-07T11:36",
            totime: "1415"
          },
          {
            friday: false,
            monday: false,
            saturday: false,
            sunday: false,
            thursday: false,
            tuesday: false,
            wednesday: true,
            created_by: 18467,
            created_on: "2015-08-07T11:38",
            doclocationid: 13982,
            doclocworkinghoursid: 6351,
            fromtime: "1245",
            modified_by: 18467,
            modified_on: "2015-08-07T11:38",
            totime: "1415"
          }
        ]
      }
    end
    render json: events_json
  end

  def book_appointment
    doc_response = {
      IsSuccess: true,
      Msg: 'add success',
      Data: '0',
      event_id: [*5..100].sample
    }
    render json: doc_response
  end

  def get_event_data
    event = params[:event]
    # if event[:event_type].eql?('booking')
      events_json = {
        id: 1,
        doctor_id: 1,
        location_id: 1,
        name: 'Just Created',
        mobile_number: '9987766554',
        email: 'foo@boo',
        # start: 'yymmdd hh:mm:ss',
        # end: 'yymmdd hh:mm:ss',
        subject: 'Some Subject',
        prepay_amount: 1234,
        # prepay_date: '2015-07-19',
        # prepay_time: '11:00:00',
        prepay_by: '2015-07-19T11:00:00',
        event_type: 'booking',
        is_paid: false,
        appointment_type: 'Patient Appointment'
      }
    # else
    #   events_json = {
    #     id: params[:id],
    #     doctor_id: 1,
    #     location_id: 1,
    #     # start: 'yymmdd hh:mm:ss',
    #     # end: 'yymmdd hh:mm:ss',
    #     subject: event[:subject],
    #     appointment_type: 'OPD',
    #     event_type: 'blocked'
    #   }
    # end
    render json: events_json
  end

  def post_event_data
    doc_response = {
      IsSuccess: true,
      Msg: 'edit success',
      Data: '0',
      event_id: rand(1..100)
    }
    render json: doc_response
  end

  def get_patients
    list = [{
      email: 'nkalnad@gmail.comlocal',
      id: 114,
      mobile: '9960439495',
      name: 'test test'
    }]
    render json: list
  end

  def todays_schedule
    render json: [
      { 'appointmentid' => 226_62, 'appointmenttype' => 'Patient Appointment',
        'time' => '18:00 to 18:15', 'starttime' => '2015-08-17T18:00:00',
        'endtime' => '2015-08-17T18:15:00', 'patientname' => 'Lekhika Abak',
        'patientmobile' => '1234123456', 'prepayamount' => 0.00,
        'ispaid' => false, 'subject' => 'Doooo FOooo',
        'prev_appt_on' => '10 Oct 2013' },
      { 'appointmentid' => 226_48, 'appointmenttype' => 'Patient Appointment',
        'time' => '18:30 to 18:45', 'starttime' => '2015-08-17T18:30:00',
        'endtime' => '2015-08-17T18:45:00', 'patientname' => 'Lekhika Abak',
        'patientmobile' => '1234123456', 'prepayamount' => 0.00,
        'ispaid' => false, 'subject' => 'Food',
        'prev_appt_on' => '10 Oct 2013' }
      ]
  end


=begin
  def get_patient_data
    render json: [
      {
        "patientid": 95,"patientname": "test kets",
        "patientemail":"kkalgaonkar+testpatient@gmail.com",
        "patientmobile":"9850648380","patienttype":"unregpatient"
      },
      {
        "patientid":99,"patientname":"abc","patientemail":"abc@gmail.com",
        "patientmobile":"9762780567","patienttype":"unregpatient"
      },
      {
        "patientid":79,"patientname":"Arif Alam",
        "patientemail":"arifjica@yahoo.inlocal","patientmobile":"9960439495",
        "patienttype":"regpatient"
      },
      {
        "patientid":94,"patientname":"ketaki",
        "patientemail":"foo@mailinator.com",
        "patientmobile":"97598757987","patienttype":"unregpatient"
      },
      {
        "patientid":114,"patientname":"priyanka",
        "patientemail":"priyanka.p@vertisinfotech.com",
        "patientmobile":"56666566666","patienttype":"unregpatient"
      },
      {
        "patientid":98,"patientname":"test",
        "patientemail":"priyanka.p@vertisinfotech.com",
        "patientmobile":"9762780567","patienttype":"unregpatient"
      },
      {
        "patientid":79,"patientname":"test kets",
        "patientemail":"kkalgaonkar+testpatient@gmail.com",
        "patientmobile":"9850648380","patienttype":"unregpatient"
      },
      {
        "patientid":17409,"patientname":"test kets",
        "patientemail":"kkalgaonkar+testpatient@gmail.com",
        "patientmobile":"9850648380","patienttype":"regpatient"
      }]
  end
=end
  private
    # Use callbacks to share common setup or constraints between actions.
    def set_post
      @post = Post.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def post_params
      params.require(:post).permit(:title, :blurb, :body)
    end
end
